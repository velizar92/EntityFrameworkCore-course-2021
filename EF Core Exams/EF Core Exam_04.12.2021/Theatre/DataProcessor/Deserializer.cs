namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {

            var serializer = new XmlSerializer(typeof(PlayDto[]), new XmlRootAttribute("Plays"));
            var playDtos = (PlayDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Play> plays = new List<Play>();

            foreach (var playDto in playDtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan oneHour = new TimeSpan(1, 0, 0);   //one hour timespan
                bool isValidDuration = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan validDuration);
                if (!isValidDuration || validDuration < oneHour)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = validDuration,
                    Rating = playDto.Rating,
                    Genre = Enum.Parse<Genre>(playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine(String.Format(SuccessfulImportPlay, playDto.Title, playDto.Genre, playDto.Rating));
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }



        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CastDto[]), new XmlRootAttribute("Casts"));
            var castDtos = (CastDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();
            List<Cast> casts = new List<Cast>();

            foreach (var castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                casts.Add(new Cast
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                });

                sb.AppendLine(String.Format(SuccessfulImportActor, castDto.FullName, castDto.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }



        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {

            List<ProjectionDto> dtoProjections = JsonConvert
                .DeserializeObject<List<ProjectionDto>>(jsonString);

            List<Theatre> importProjections = new List<Theatre>();
            StringBuilder output = new StringBuilder();

            foreach (var dtoProjection in dtoProjections)
            {
                if (!IsValid(dtoProjection))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                List<Ticket> tickets = new List<Ticket>();
                foreach (var dtoTicket in dtoProjection.Tickets)
                {
                    if (!IsValid(dtoTicket))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    tickets.Add(
                        new Ticket
                        {
                            Price = dtoTicket.Price,
                            RowNumber = dtoTicket.RowNumber,
                            PlayId = dtoTicket.PlayId
                        }
                            );
                }

                importProjections.Add(
                    new Theatre
                    {
                        Name = dtoProjection.Name,
                        NumberOfHalls = dtoProjection.NumberOfHalls,
                        Director = dtoProjection.Director,
                        Tickets = tickets
                    });

                output.AppendLine(string.Format(SuccessfulImportTheatre, dtoProjection.Name, tickets.Count));
            }

            context.Theatres.AddRange(importProjections);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }



        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}

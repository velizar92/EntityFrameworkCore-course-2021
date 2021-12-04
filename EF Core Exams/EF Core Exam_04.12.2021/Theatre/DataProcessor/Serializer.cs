namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {

            var teatres = context
                .Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price),
                    Tickets = t.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Select(ticket => new
                    {
                        Price = ticket.Price,
                        RowNumber = ticket.RowNumber
                    })
                    .OrderByDescending(t => t.Price)
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(teatres, Formatting.Indented);
        }



        public static string ExportPlays(TheatreContext context, double rating)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayExportDto[]), new XmlRootAttribute("Plays"));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using StringWriter sw = new StringWriter(sb);

            var result = context
                .Plays
                .ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new PlayExportDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),                 
                    Rating = p.Rating == 0.00 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts.Where(c => c.IsMainCharacter == true)
                    .Select(c => new ActorDto
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'.",
                    })
                     .OrderByDescending(p => p.FullName)
                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();


            xmlSerializer.Serialize(sw, result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}

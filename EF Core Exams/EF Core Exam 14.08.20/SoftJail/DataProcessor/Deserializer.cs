namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
          
            StringBuilder output = new StringBuilder();
            var departmentDtos = JsonConvert
                        .DeserializeObject<DepartmentInputDto[]>(jsonString);
            List<Department> departrments = new List<Department>();

            foreach (var departmentDto in departmentDtos)
            {
                if(!IsValid(departmentDto) || departmentDto.Cells.Length == 0 
                    || departmentDto.Cells.Any(d => !IsValid(d)))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }
                
                Department department = new Department()
                {
                    Name = departmentDto.Name
                };

                foreach (var cell in departmentDto.Cells)
                {
                    var cellObj = new Cell()
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow
                    };

                    department.Cells.Add(cellObj);
                }

                departrments.Add(department);
                output.AppendLine($"Imported {departmentDto.Name} with {departmentDto.Cells.Length} cells");
            }
      
            context.Departments.AddRange(departrments);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }



        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {

            StringBuilder output = new StringBuilder();
            var prisonerDtos = JsonConvert
                        .DeserializeObject<PrisonerInputDto[]>(jsonString);
            List<Prisoner> prisoners = new List<Prisoner>();


            foreach(var prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto) || !IsValid(prisonerDto.Mails.Any(m => !IsValid(m))))
                {
                    output.AppendLine("Invalid Data");
                    continue;
                }
              

                DateTime incarcerationDate;
                bool isIncarcerationDate = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                DateTime releaseDate;
                bool isReleaseDate = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                var prisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                foreach (var mailDto in prisonerDto.Mails)
                {
                    var mail = new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };

                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                output.AppendLine($"Imported {prisonerDto.FullName} {prisonerDto.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);

            context.SaveChanges();


            return output.ToString().TrimEnd();
        }



        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            return "";
        }



        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
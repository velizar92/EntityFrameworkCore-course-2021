namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisioners = context
                .Prisoners
                .Where(p => ids.Contains(p.Id))
                .ToArray()
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(op => new
                    {
                        OfficerName = op.Officer.FullName,
                        Department = op.Officer.Department.Name
                    })
                    .OrderBy(op => op.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers
                    .Sum(x => x.Officer.Salary).ToString("F2"))
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();


            return JsonConvert.SerializeObject(prisioners, Formatting.Indented);
        }



        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {

            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(PrisonerDto[]),
                    new XmlRootAttribute("Prisoners"));

            using StringWriter sw = new StringWriter(sb);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var prisoners = prisonersNames.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var resultPrisoners = context
                .Prisoners
                .Where(p => prisoners.Contains(p.FullName))
                .Select(p => new PrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new MessageDto
                    {
                        Description = Reverse(m.Description)
                    })
                    .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            xmlSerializer.Serialize(sw, resultPrisoners, ns);

            return sb.ToString().TrimEnd();
        }




        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
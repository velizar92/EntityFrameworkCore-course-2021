namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

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
                    TotalOfficerSalary = $"{p.PrisonerOfficers.Select(op => op.Officer.Salary).Sum():f2}"
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            
            return JsonConvert.SerializeObject(prisioners, Formatting.Indented); 
        }



        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            return "";
        }
    }
}
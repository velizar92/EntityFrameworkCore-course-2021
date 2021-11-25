namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectModel[]), new XmlRootAttribute("Projects"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            var projects = context
                .Projects
                .Where(p => p.Tasks.Any())              //If it has at least one task
                .ToArray()
                .Select(p => new ExportProjectModel()
                {
                    Name = p.Name,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    TasksCount = p.Tasks.Count,
                    Tasks = p.Tasks
                    .ToArray()
                    .Select(t => new ExportTaskModel()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.Name)
                .ToArray();

            xmlSerializer.Serialize(sw, projects, namespaces);

            return sb.ToString().TrimEnd();
        }



        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {

            var employees = context
               .Employees
               .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
               .ToArray()
               .Select(e => new
               {
                   e.Username,
                   Tasks = e.EmployeesTasks
                   .Where(et => et.Task.OpenDate >= date)
                   .OrderByDescending(et => et.Task.DueDate)
                   .ThenBy(et => et.Task.Name)
                   .Select(et => new
                   {
                       TaskName = et.Task.Name,
                       OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                       DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                       LabelType = et.Task.LabelType.ToString(),
                       ExecutionType = et.Task.ExecutionType.ToString(),
                   })
                   .ToArray()
               })
               .OrderByDescending(e => e.Tasks.Length)
               .ThenBy(e => e.Username)
               .Take(10)
               .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }

    }
}
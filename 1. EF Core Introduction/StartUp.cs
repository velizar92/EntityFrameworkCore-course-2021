namespace SoftUni
{
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Data;
    using SoftUni.Models;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {

            SoftUniContext context = new SoftUniContext();

            var employees = GetEmployee147(context);
            Console.WriteLine(employees);
        }


        // Task 9 -> Employee 147 
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147Data = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name
                    })
                })
               
                .FirstOrDefault();

            sb.AppendLine($"{employee147Data.FirstName} {employee147Data.LastName} - {employee147Data.JobTitle}");
           
            foreach (var project in employee147Data.Projects.OrderBy(p => p.ProjectName))
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }





        // Task 8 -> Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
               .Select(x => new
               {
                   AddressText = x.AddressText,
                   TownName = x.Town.Name,
                   EmployeeCount = x.Employees.Count
               })
               .OrderByDescending(a => a.EmployeeCount)
               .ThenBy(a => a.TownName)
               .ThenBy(a => a.AddressText)
               .Take(10)
               .ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }



        // Task 7 -> Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesInPeriod = context.Employees
              .Where(e => e.EmployeesProjects
              .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
              .Select(e => new
              {
                  FirstName = e.FirstName,
                  LastName = e.LastName,
                  ManagerFirstName = e.Manager.FirstName,
                  ManagerLastName = e.Manager.LastName,
                  Projects = e.EmployeesProjects.Select(ep => new
                  {
                      ProjectName = ep.Project.Name,
                      ProjectStartDate = ep.Project.StartDate,
                      ProjectEndDate = ep.Project.EndDate
                  })
              })
              .Take(10);

            foreach (var employee in employeesInPeriod)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");
                foreach (var project in employee.Projects)
                {

                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";                  //used ternar operator

                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");

                }
            }

            return sb.ToString().TrimEnd();
        }




        // Task 6 -> Adding a new address and Updating an Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var address = new Address();
            address.AddressText = "Vitoshka 15";
            address.TownId = 4;
            context.Addresses.Add(address);

            var searchedStudent = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            searchedStudent.Address = address;

            context.SaveChanges();

            var resultAddresses = context.Employees
                .Select(a => new
                {
                    AddressId = a.Address.AddressId,
                    AddressText = a.Address.AddressText
                })
                .OrderByDescending(a => a.AddressId)
                .Take(10)
                .ToList();

            foreach (var addr in resultAddresses)
            {
                sb.AppendLine(addr.AddressText);
            }


            return sb.ToString();
        }



        // Task 5 -> Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
               .Where(e => e.Department.Name == "Research and Development")
               .Select(e => new
               {
                   FirstName = e.FirstName,
                   LastName = e.LastName,
                   DepartmentName = e.Department.Name,
                   Salary = e.Salary
               })
               .OrderBy(e => e.Salary)
               .ThenByDescending(e => e.FirstName)
               .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }

            return sb.ToString();
        }


        // Task 4 -> Employees with Salary over than 50000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString();
        }


        // Task 3 -> Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.EmployeeId)
                .ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString();
        }


    }
}

﻿namespace SoftUni
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

            var employees = AddNewAddressToEmployee(context);
            Console.WriteLine(employees);
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
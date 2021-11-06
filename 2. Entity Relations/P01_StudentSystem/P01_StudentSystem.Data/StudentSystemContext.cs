using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }


        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {

        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Homework> HomeworkSubmissions { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
               .Property(b => b.PhoneNumber)
               .IsUnicode(false);

            modelBuilder.Entity<Resource>()
              .Property(b => b.Url)
              .IsUnicode(false);

            modelBuilder.Entity<Homework>()
            .Property(b => b.Content)
            .IsUnicode(false);

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(sc => new { sc.StudentId, sc.CourseId });
            });
        }



    }
}

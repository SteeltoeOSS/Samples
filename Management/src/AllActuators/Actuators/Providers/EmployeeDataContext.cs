using Bogus;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Actuators.Models;
using System.Collections.Generic;

namespace Steeltoe.Actuators.Providers
{
    public class EmployeeDataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDataContext(DbContextOptions<EmployeeDataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>()
                .HasData(GenerateEmployeData());
        }

        private IEnumerable<Employee> GenerateEmployeData()
        {
            var employeeCount = new Faker().Random.Number(50, 100);

            return new Faker<Employee>()
                .RuleFor(employee => employee.Id, fake => fake.Random.Guid())
                .RuleFor(employee => employee.FirstName, fake => fake.Name.FirstName())
                .RuleFor(employee => employee.LastName, fake => fake.Name.LastName())
                .RuleFor(employee => employee.Company, fake => fake.Company.CompanyName())
                .RuleFor(employee => employee.Title, fake => fake.Company.Bs())
                .Generate(employeeCount);
        }
    }
}

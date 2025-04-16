using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.DOMAINE.Entities;
using LeaveManagementSystem.DOMAINE.Enums;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.DATA.Migrations
{
    public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sample data seeding
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FullName = "Khalil Frikha", Department = "IT", JoiningDate = new DateTime(2024, 1, 1) }
        );

        modelBuilder.Entity<LeaveRequest>().HasData(
            new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveType = LeaveType.Annual,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                Status = LeaveStatus.Pending,
                Reason = "Family needs",
                CreatedAt = new DateTime(2024, 4, 16)
            }
        );
    }
}

}

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
            new Employee { Id = 1, FullName = "Khalil Frikha", Department = "IT", JoiningDate = new DateTime(2024, 1, 1) },
            new Employee { Id = 2, FullName = "Ahmed", Department = "IT", JoiningDate = new DateTime(2024, 1, 1) },
            new Employee { Id = 3, FullName = "Heni", Department = "IT", JoiningDate = new DateTime(2024, 1, 1) },
            new Employee { Id = 4, FullName = "Mohamed", Department = "HR", JoiningDate = new DateTime(2024, 1, 1) },
            new Employee { Id = 5, FullName = "Aicha", Department = "HR", JoiningDate = new DateTime(2024, 1, 1) },
            new Employee { Id = 6, FullName = "Nada", Department = "Finance", JoiningDate = new DateTime(2024, 1, 1) }
        );

        modelBuilder.Entity<LeaveRequest>().HasData(
            new LeaveRequest
            {
                Id = 1,
                EmployeeId = 1,
                LeaveType = LeaveType.Annual,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(5),
                Status = LeaveStatus.Approved,
                Reason = "Family needs",
                CreatedAt = new DateTime(2024, 4, 16)
            },
             new LeaveRequest
             {
                 Id = 2,
                 EmployeeId = 5,
                 LeaveType = LeaveType.Annual,
                 StartDate = DateTime.Today,
                 EndDate = DateTime.Today.AddDays(5),
                 Status = LeaveStatus.Approved,
                 Reason = null,
                 CreatedAt = new DateTime(2024, 4, 16)
             }
        );
    }
}

}

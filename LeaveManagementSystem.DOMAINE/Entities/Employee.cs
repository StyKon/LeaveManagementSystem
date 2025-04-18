using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.DOMAINE.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }

        public ICollection<LeaveRequest>? LeaveRequests { get; set; }
    }
}

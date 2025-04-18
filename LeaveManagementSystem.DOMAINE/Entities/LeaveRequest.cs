using System.ComponentModel.DataAnnotations;
using LeaveManagementSystem.DOMAINE.Enums;

namespace LeaveManagementSystem.DOMAINE.Entities
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [EnumDataType(typeof(LeaveType))]
        public LeaveType LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [EnumDataType(typeof(LeaveStatus))]
        public LeaveStatus Status { get; set; }

        public string? Reason { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public Employee Employee { get; set; }

    }
}

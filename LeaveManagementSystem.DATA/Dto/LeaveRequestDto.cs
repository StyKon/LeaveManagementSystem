using System.ComponentModel.DataAnnotations;
using LeaveManagementSystem.DOMAINE.Enums;

namespace LeaveManagementSystem.DATA.Dto
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [EnumDataType(typeof(LeaveType), ErrorMessage = "Invalid leave type.")]
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [EnumDataType(typeof(LeaveStatus), ErrorMessage = "Invalid leave status.")]
        public LeaveStatus Status { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using LeaveManagementSystem.DOMAINE.Enums;

namespace LeaveManagementSystem.DATA.Dto
{
    public class LeaveRequestFilterDto
    {
        public int? employeeId { get; set; }
        public LeaveType? leaveType { get; set; }
        public LeaveStatus? status { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public int page { get; set; } = 1;

        private int _pageSize = 10;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : 10;
        }

        public string? sortBy { get; set; } = "CreatedAt";
        public string? sortOrder { get; set; } = "desc";
        public string? keyword { get; set; }
    }

}

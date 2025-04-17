using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.DOMAINE.Enums;

namespace LeaveManagementSystem.DATA.Dto
{
    public class LeaveRequestFilterDto
    {
        public int? EmployeeId { get; set; }
        public LeaveType? LeaveType { get; set; }
        public LeaveStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : 10;
        }

        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortOrder { get; set; } = "desc";

        public string? Keyword { get; set; }
    }

}

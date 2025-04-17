
namespace LeaveManagementSystem.DATA.Dto
{
    public class LeaveReportFilterDto
    {
        public int year { get; set; }
        public string? department { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }

}

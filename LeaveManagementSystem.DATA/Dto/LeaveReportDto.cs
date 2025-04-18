namespace LeaveManagementSystem.DATA.Dto
{
    public class LeaveReportDto
    {
        public EmployeeDto Employee { get; set; }
        public int TotalLeaves { get; set; }
        public int AnnualLeaves { get; set; }
        public int SickLeaves { get; set; }
    }
}

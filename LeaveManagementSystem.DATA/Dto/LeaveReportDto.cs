using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

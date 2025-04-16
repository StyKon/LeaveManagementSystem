using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.DOMAINE.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}

using LeaveManagementSystem.DATA.Repositories.Interfaces;
using LeaveManagementSystem.DOMAINE.Entities;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.DATA.Repositories
{
    public class LeaveRequestRepository : BaseRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(DbContext context) : base(context)
        {
        }
    }
}

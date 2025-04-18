using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DOMAINE.Entities;

namespace LeaveManagementSystem.DATA.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<EntityResult<int>> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<EntityResult<IEnumerable<LeaveRequest>>> GetAllLeaveRequestsAsync();
        Task<EntityResult<LeaveRequest>> GetLeaveRequestByIdAsync(int id);
        Task<EntityResult<int>> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<EntityResult<int>> DeleteLeaveRequestAsync(int id);
        Task<EntityResult<int>> DeleteLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<EntityResult<PagedResult<LeaveRequest>>> FilterLeaveRequestsAsync(LeaveRequestFilterDto filterDto);
        Task<IEnumerable<LeaveReportDto>> GetLeaveReportAsync(LeaveReportFilterDto filter);
        Task<EntityResult<int>> ApproveLeaveRequestAsync(int id);

    }
}

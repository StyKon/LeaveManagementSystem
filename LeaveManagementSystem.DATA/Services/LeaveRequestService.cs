using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Repositories;
using LeaveManagementSystem.DOMAINE.Entities;

namespace LeaveManagementSystem.DATA.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }


        public async Task<EntityResult<int>> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
                return EntityResult<int>.FailureResult("Leave request is null.");

            try
            {
                var result = await _leaveRequestRepository.InsertAsync(leaveRequest);
                return EntityResult<int>.SuccessResult(result, "Leave request created successfully.");
            }
            catch (Exception ex)
            {
                return EntityResult<int>.FailureResult($"Error creating leave request: {ex.Message}");
            }
        }

        public async Task<EntityResult<IEnumerable<LeaveRequest>>> GetAllLeaveRequestsAsync()
        {
            try
            {
                var data = await _leaveRequestRepository.GetAllAsync();
                return EntityResult<IEnumerable<LeaveRequest>>.SuccessResult(data, "Fetched leave requests.");
            }
            catch (Exception ex)
            {
                return EntityResult<IEnumerable<LeaveRequest>>.FailureResult($"Error fetching leave requests: {ex.Message}");
            }
        }

        public async Task<EntityResult<LeaveRequest>> GetLeaveRequestByIdAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                return EntityResult<LeaveRequest>.FailureResult($"Leave request with ID {id} not found.");

            return EntityResult<LeaveRequest>.SuccessResult(leaveRequest, "Leave request found.");
        }

        public async Task<EntityResult<int>> UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
                return EntityResult<int>.FailureResult("Leave request is null.");

            try
            {
                var result = await _leaveRequestRepository.UpdateAsync(leaveRequest);
                return EntityResult<int>.SuccessResult(result, "Leave request updated successfully.");
            }
            catch (Exception ex)
            {
                return EntityResult<int>.FailureResult($"Error updating leave request: {ex.Message}");
            }
        }

        public async Task<EntityResult<int>> DeleteLeaveRequestAsync(int id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                return EntityResult<int>.FailureResult($"Leave request with ID {id} not found.");

            return await DeleteLeaveRequestAsync(leaveRequest);
        }

        public async Task<EntityResult<int>> DeleteLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
                return EntityResult<int>.FailureResult("Leave request is null.");

            try
            {
                var result = await _leaveRequestRepository.DeleteAsync(leaveRequest);
                return EntityResult<int>.SuccessResult(result, "Leave request deleted successfully.");
            }
            catch (Exception ex)
            {
                return EntityResult<int>.FailureResult($"Error deleting leave request: {ex.Message}");
            }
        }

    }
    public interface ILeaveRequestService
    {
        Task<EntityResult<int>> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<EntityResult<IEnumerable<LeaveRequest>>> GetAllLeaveRequestsAsync();
        Task<EntityResult<LeaveRequest>> GetLeaveRequestByIdAsync(int id);
        Task<EntityResult<int>> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<EntityResult<int>> DeleteLeaveRequestAsync(int id);
        Task<EntityResult<int>> DeleteLeaveRequestAsync(LeaveRequest leaveRequest);
    }

}

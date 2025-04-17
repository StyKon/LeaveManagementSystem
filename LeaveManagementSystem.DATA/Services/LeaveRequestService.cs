using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DATA.Repositories;
using LeaveManagementSystem.DOMAINE.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<EntityResult<PagedResult<LeaveRequest>>> FilterLeaveRequestsAsync(LeaveRequestFilterDto filterDto)
        {
            try
            {
                var query = _leaveRequestRepository.GetZ(x =>
                    (filterDto.EmployeeId == null || x.EmployeeId == filterDto.EmployeeId) &&
                    (filterDto.Status == null || x.Status == filterDto.Status) &&
                    (filterDto.LeaveType == null || x.LeaveType == filterDto.LeaveType) &&
                    (filterDto.StartDate == null || x.StartDate >= filterDto.StartDate) &&
                    (filterDto.EndDate == null || x.EndDate <= filterDto.EndDate) &&
                    (filterDto.Keyword == null || x.Reason.Contains(filterDto.Keyword))
                );

                var totalItems = await query.CountAsync();


                if (filterDto.SortBy?.ToLower() == "startdate")
                {
                    query = filterDto.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(x => x.StartDate)
                        : query.OrderBy(x => x.StartDate);
                }
                else
                {
                    query = filterDto.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(x => x.CreatedAt)
                        : query.OrderBy(x => x.CreatedAt);
                }

                query = query
                    .Skip(Math.Max(0, (filterDto.Page - 1)) * filterDto.PageSize)
                    .Take(filterDto.PageSize);

                var items = await query.ToListAsync();


                var pagedResult = new PagedResult<LeaveRequest>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = filterDto.Page,
                    PageSize = filterDto.PageSize
                };

                return EntityResult<PagedResult<LeaveRequest>>.SuccessResult(pagedResult, "Filtered results retrieved.");
            }
            catch (Exception ex)
            {
                return EntityResult<PagedResult<LeaveRequest>>.FailureResult($"Error during filtering: {ex.Message}");
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
        Task<EntityResult<PagedResult<LeaveRequest>>> FilterLeaveRequestsAsync(LeaveRequestFilterDto filterDto);

    }

}

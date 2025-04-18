using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DATA.Repositories.Interfaces;
using LeaveManagementSystem.DATA.Services.Interfaces;
using LeaveManagementSystem.DOMAINE.Entities;
using LeaveManagementSystem.DOMAINE.Enums;
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
                var validationResult = await ValidateLeaveRequestAsync(leaveRequest);
                if (!validationResult.Success)
                    return validationResult;

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
            try
            {
                var existing = await _leaveRequestRepository.GetByIdAsync(leaveRequest.Id);
                if (existing == null)
                    return EntityResult<int>.FailureResult("Leave request not found.");

                var validationResult = await ValidateLeaveRequestAsync(leaveRequest);
                if (!validationResult.Success)
                    return validationResult;


                existing.StartDate = leaveRequest.StartDate;
                existing.EndDate = leaveRequest.EndDate;
                existing.LeaveType = leaveRequest.LeaveType;
                existing.Reason = leaveRequest.Reason;
                existing.Status = leaveRequest.Status;
                existing.EmployeeId = leaveRequest.EmployeeId;

                var result = await _leaveRequestRepository.UpdateAsync(existing);
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
                    (filterDto.employeeId == null || x.EmployeeId == filterDto.employeeId) &&
                    (filterDto.status == null || x.Status == filterDto.status) &&
                    (filterDto.leaveType == null || x.LeaveType == filterDto.leaveType) &&
                    (filterDto.startDate == null || x.StartDate >= filterDto.startDate) &&
                    (filterDto.endDate == null || x.EndDate <= filterDto.endDate) &&
                    (filterDto.keyword == null || x.Reason.Contains(filterDto.keyword))
                );

                var totalItems = await query.CountAsync();


                switch (filterDto.sortBy?.ToLower())
                {
                    case "startdate":
                        query = filterDto.sortOrder == "desc" ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate);
                        break;
                    case "enddate":
                        query = filterDto.sortOrder == "desc" ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate);
                        break;
                    default:
                        query = filterDto.sortOrder == "desc" ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);
                        break;
                }

                query = query
                    .Skip(Math.Max(0, (filterDto.page - 1)) * filterDto.pageSize)
                    .Take(filterDto.pageSize);

                var items = await query.ToListAsync();


                var pagedResult = new PagedResult<LeaveRequest>
                {
                    Items = items,
                    TotalItems = totalItems,
                    Page = filterDto.page,
                    PageSize = filterDto.pageSize
                };

                return EntityResult<PagedResult<LeaveRequest>>.SuccessResult(pagedResult, "Filtered results retrieved.");
            }
            catch (Exception ex)
            {
                return EntityResult<PagedResult<LeaveRequest>>.FailureResult($"Error during filtering: {ex.Message}");
            }
        }
        public async Task<IEnumerable<LeaveReportDto>> GetLeaveReportAsync(LeaveReportFilterDto filter)
        {
            var query = _leaveRequestRepository.GetZ(l =>
                l.StartDate.Year == filter.year &&
                (filter.fromDate == null || l.EndDate >= filter.fromDate) &&
                (filter.toDate == null || l.StartDate <= filter.toDate) &&
                l.Status == LeaveStatus.Approved
            );

            if (!string.IsNullOrEmpty(filter.department))
                query = query.Include(x => x.Employee).Where(l => l.Employee.Department == filter.department);
            else
                query = query.Include(x => x.Employee);

            var leaveList = await query.ToListAsync();

            int GetOverlapDays(LeaveRequest leave)
            {
                var start = filter.fromDate.HasValue && leave.StartDate < filter.fromDate ? filter.fromDate.Value : leave.StartDate;
                var end = filter.toDate.HasValue && leave.EndDate > filter.toDate ? filter.toDate.Value : leave.EndDate;
                return (end - start).Days + 1;
            }

            var report = leaveList
                .GroupBy(l => l.Employee.Id)
                .Select(group => BuildLeaveReport(group, GetOverlapDays))
                .ToList();

            return report;
        }

        public async Task<EntityResult<int>> ApproveLeaveRequestAsync(int id)
        {
            var leave = await _leaveRequestRepository.GetByIdAsync(id);

            if (leave == null)
                return EntityResult<int>.FailureResult("Leave request not found.");

            if (leave.Status != LeaveStatus.Pending)
                return EntityResult<int>.FailureResult("Only pending requests can be approved.");

            leave.Status = LeaveStatus.Approved;
            var result = await _leaveRequestRepository.UpdateAsync(leave);

            return EntityResult<int>.SuccessResult(result, "Leave request approved.");
        }
        private LeaveReportDto BuildLeaveReport(IGrouping<int, LeaveRequest> group, Func<LeaveRequest, int> getOverlapDays)
        {
            var employee = group.First().Employee;

            return new LeaveReportDto
            {
                Employee = new EmployeeDto
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Department = employee.Department,
                    JoiningDate = employee.JoiningDate
                },
                TotalLeaves = group.Sum(getOverlapDays),
                AnnualLeaves = group
                    .Where(l => l.LeaveType == LeaveType.Annual)
                    .Sum(getOverlapDays),
                SickLeaves = group
                    .Where(l => l.LeaveType == LeaveType.Sick)
                    .Sum(getOverlapDays)
            };
        }
        private async Task<EntityResult<int>> ValidateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            if (await IsOverlappingLeaveDatesAsync(leaveRequest))
                return EntityResult<int>.FailureResult("Employee already has overlapping leave during this period.");

            if (leaveRequest.LeaveType == LeaveType.Annual &&
                await HasExceededAnnualLeaveLimitAsync(leaveRequest))
                return EntityResult<int>.FailureResult("Annual leave limit of 20 days per year exceeded.");

            if (IsSickLeaveReasonInvalid(leaveRequest))
                return EntityResult<int>.FailureResult("Sick leave requires a valid reason.");

            return EntityResult<int>.SuccessResult(0);
        }

        private async Task<bool> IsOverlappingLeaveDatesAsync(LeaveRequest leaveRequest)
        {
            return await _leaveRequestRepository
                .GetZ(x => x.EmployeeId == leaveRequest.EmployeeId &&
                   (x.Id != leaveRequest.Id || leaveRequest.Id == 0) &&
                           x.StartDate <= leaveRequest.EndDate &&
                           x.EndDate >= leaveRequest.StartDate)
                .AnyAsync();
        }
        private async Task<bool> HasExceededAnnualLeaveLimitAsync(LeaveRequest leaveRequest)
        {
            var year = leaveRequest.StartDate.Year;

            var existingAnnualLeaves = await _leaveRequestRepository
                .GetZ(x => x.EmployeeId == leaveRequest.EmployeeId &&
                           x.LeaveType == LeaveType.Annual &&
                           x.StartDate.Year == year &&
                   (x.Id != leaveRequest.Id || leaveRequest.Id == 0)
                   )
                .ToListAsync();

            var annualLeaveDaysTaken = existingAnnualLeaves.Sum(x =>
                (x.EndDate - x.StartDate).Days + 1
            );

            var requestedDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            return (annualLeaveDaysTaken + requestedDays) > 20;
        }
        private bool IsSickLeaveReasonInvalid(LeaveRequest leaveRequest)
        {
            return leaveRequest.LeaveType == LeaveType.Sick &&
                   string.IsNullOrWhiteSpace(leaveRequest.Reason);
        }

    }

}

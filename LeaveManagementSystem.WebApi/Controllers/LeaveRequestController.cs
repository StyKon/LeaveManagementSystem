using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DATA.Services;
using LeaveManagementSystem.DOMAINE.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [ApiController]
    [Route("api/leaverequests")]
    public class LeaveRequestController : ControllerBase
    {  

        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILogger<LeaveRequestController> logger,ILeaveRequestService leaveRequestService)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> Get()
        {
            var result = await _leaveRequestService.GetAllLeaveRequestsAsync();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var leaveRequestDtos = result.Data.Select(x => new LeaveRequestDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                EmployeeId = x.EmployeeId,
                EndDate = x.EndDate,
                LeaveType = x.LeaveType,
                Reason = x.Reason,
                StartDate = x.StartDate,
                Status = x.Status
            });

            return Ok(leaveRequestDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDto>> GetLeaveRequest(int id)
        {
            var result = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                return NotFound(result.Message);
            }

            var dto = new LeaveRequestDto
            {
                Id = result.Data.Id,
                CreatedAt = result.Data.CreatedAt,
                EmployeeId = result.Data.EmployeeId,
                EndDate = result.Data.EndDate,
                LeaveType = result.Data.LeaveType,
                Reason = result.Data.Reason,
                StartDate = result.Data.StartDate,
                Status = result.Data.Status
            };

            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {
            if (leaveRequestDto == null)
                return BadRequest("Leave request data is missing.");

            var leaveRequest = new LeaveRequest
            {
                CreatedAt = DateTime.UtcNow,
                EmployeeId = leaveRequestDto.EmployeeId,
                EndDate = leaveRequestDto.EndDate,
                LeaveType = leaveRequestDto.LeaveType,
                Reason = leaveRequestDto.Reason,
                StartDate = leaveRequestDto.StartDate,
                Status = leaveRequestDto.Status
            };

            var result = await _leaveRequestService.CreateLeaveRequestAsync(leaveRequest);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetLeaveRequest), new { id = leaveRequest.Id }, leaveRequestDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequestDto leaveRequestDto)
        {
            if (leaveRequestDto == null || id != leaveRequestDto.Id)
                return BadRequest("Invalid leave request data.");

            var existingResult = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            if (!existingResult.Success || existingResult.Data == null)
                return NotFound(existingResult.Message);

            var updatedLeaveRequest = existingResult.Data;
            updatedLeaveRequest.EndDate = leaveRequestDto.EndDate;
            updatedLeaveRequest.LeaveType = leaveRequestDto.LeaveType;
            updatedLeaveRequest.Reason = leaveRequestDto.Reason;
            updatedLeaveRequest.StartDate = leaveRequestDto.StartDate;
            updatedLeaveRequest.Status = leaveRequestDto.Status;

            var updateResult = await _leaveRequestService.UpdateLeaveRequestAsync(updatedLeaveRequest);

            if (!updateResult.Success)
                return BadRequest(updateResult.Message);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var result = await _leaveRequestService.DeleteLeaveRequestAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

    }
}

using AutoMapper;
using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DATA.Services.Interfaces;
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
        private readonly IMapper _mapper;

        public LeaveRequestController(ILogger<LeaveRequestController> logger,
            ILeaveRequestService leaveRequestService,
            IMapper mapper)
        {
            _logger = logger;
            _leaveRequestService = leaveRequestService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequestDto>>> Get()
        {
            var result = await _leaveRequestService.GetAllLeaveRequestsAsync();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            var leaveRequestDtos = _mapper.Map<IEnumerable<LeaveRequestDto>>(result.Data);

            return Ok(leaveRequestDtos);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] LeaveRequestFilterDto filterDto)
        {
            var result = await _leaveRequestService.FilterLeaveRequestsAsync(filterDto);
            if (!result.Success) return BadRequest(result.Message);

            var dtoResult = _mapper.Map<PagedResult<LeaveRequestDto>>(result.Data);
            return Ok(dtoResult);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDto>> GetLeaveRequest(int id)
        {
            var result = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                return NotFound(result.Message);
            }

            var dto = _mapper.Map<LeaveRequestDto>(result.Data);

            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestDto leaveRequestDto)
        {
            if (leaveRequestDto == null)
                return BadRequest("Leave request data is missing.");

            if (leaveRequestDto.EndDate < leaveRequestDto.StartDate)
                return BadRequest("End date must be after start date.");

            if (leaveRequestDto.StartDate < DateTime.Today)
                return BadRequest("Start date cannot be in the past.");

            var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDto);

            leaveRequest.CreatedAt = DateTime.Now;

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

            if (leaveRequestDto.EndDate < leaveRequestDto. StartDate)
                return BadRequest("End date must be after start date.");

            var updatedLeaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDto);
            updatedLeaveRequest.Id = id;

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

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] LeaveReportFilterDto filter)
        {
            var result = await _leaveRequestService.GetLeaveReportAsync(filter);

            return Ok(result);
        }


        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _leaveRequestService.ApproveLeaveRequestAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


    }
}

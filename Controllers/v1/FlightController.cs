using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityTuitionPaymentV2.Model.Constants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private IFlightService _flightService;
        private IDistanceCalculatorService _distanceCalculatorService;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightService service, IDistanceCalculatorService distanceCalculatorService, ILogger<FlightController> logger)
        {
            _flightService = service ?? throw new ArgumentNullException(nameof(service));
            _distanceCalculatorService = distanceCalculatorService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /*
         * [HttpPost("UnpaidTuitionStatus")]
        public List<StudentDto> GetUnpaidTuitionStatus([FromBody] QueryWithPagingDto query)
        {
            List<Student> datas = _studentService.Get().ToList();
            List<Student> datasFiltered = datas.Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize).ToList();

            List<StudentDto> ret = new List<StudentDto>();
            datasFiltered.ForEach(data => { if(data.Status == StudentStatus.PaymentPending) ret.Add(createStudentDto(data)); });
            return ret;
        }
         */

        [HttpPost("POST/Filtered")]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetFlights([FromBody] FlightFilterModel? flightFilterModel)
        {
            try
            {
                var flights = await _flightService.GetFlightsAsync(flightFilterModel);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<FlightDto>>> GetFlights()
        {
            try
            {
                var flights = await _flightService.GetFlightsAsync();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<FlightDto>> GetFlight(int id)
        {
            try
            {
                var flightDto = await _flightService.GetFlightAsync(id);

                if (flightDto == null)
                {
                    return NotFound();
                }

                return Ok(flightDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/CODE/{code}")]
        public async Task<ActionResult<FlightDto>> GetFlightByCode(string code)
        {
            try
            {
                var flightDto = await _flightService.GetFlightByCodeAsync(code);

                if (flightDto == null)
                {
                    return NotFound();
                }

                return Ok(flightDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        [Authorize]
        public async Task<ActionResult<FlightDto>> InsertFlight([FromBody] FlightDto flightDto)
        {
            if (flightDto == null)
            {
                return BadRequest("Flight data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Flight flightNew = await _flightService.InsertFlightAsync(flightDto);
                return CreatedAtAction(nameof(InsertFlight), new { id = flightNew.Id }, flightDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating flight");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightDto flightDto)
        {
            if (flightDto == null)
            {
                return BadRequest("Flight data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingFlight = await _flightService.GetFlightAsync(id);

                if (existingFlight == null)
                {
                    return NotFound();
                }

                await _flightService.UpdateFlightAsync(id, flightDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating flight");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                var existingFlight = await _flightService.GetFlightAsync(id);

                if (existingFlight == null)
                {
                    return NotFound();
                }

                await _flightService.DeleteFlightAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting flight");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeleteFlights()
        {
            try
            {
                await _flightService.DeleteFlightsAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting flights");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

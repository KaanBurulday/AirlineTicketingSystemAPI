using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private IAirportService _airportService;
        private IPlaneService _planeService;
        private readonly ILogger<AirportController> _logger;

        public AirportController(IAirportService service, IPlaneService planeService, ILogger<AirportController> logger)
        {
            _airportService = service ?? throw new ArgumentNullException(nameof(service));
            _planeService = planeService ?? throw new ArgumentNullException(nameof(planeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<AirportDto>>> GetAirportsDtoAsync()
        {
            try
            {
                var airports = await _airportService.GetAirportsAsync();
                return Ok(airports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/ID/{id}")]
        public async Task<ActionResult<AirportDto>> GetAirportAsync(int id)
        {
            try
            {
                var airportDto = await _airportService.GetAirportAsync(id);

                if (airportDto == null)
                {
                    return NotFound();
                }

                return Ok(airportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/CODE/{code}")]
        public async Task<ActionResult<AirportDto>> GetAirportAsync(string code)
        {
            try
            {
                var airport = await _airportService.GetAirportByCodeAsync(code);

                if (airport == null)
                {
                    return NotFound();
                }

                return Ok(airport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{code}/Planes")]
        public async Task<ActionResult<List<PlaneDto>>> GetPlanesOfAirport(string code)
        {
            try
            {
                var airportDto = await _airportService.GetAirportByCodeAsync(code);

                if (airportDto == null)
                {
                    return NotFound();
                }

                List<PlaneDto> planes = new List<PlaneDto>();
                foreach(var planeCode in airportDto.PlaneCodes)
                {
                    planes.Add(await _planeService.GetPlaneByCodeAsync(planeCode));
                }
                return Ok(planes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving planes of airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{code}/AvAirports")]
        public async Task<ActionResult<List<AirportDto>>> GetAvAirportsOfAirport(string code)
        {
            try
            {
                var airportDto = await _airportService.GetAirportByCodeAsync(code);

                if (airportDto == null)
                {
                    return NotFound();
                }

                List<AirportDto> avAirports = new List<AirportDto>();
                foreach (var airportCode in airportDto.AvailableAirportsCodes)
                {
                    avAirports.Add(await _airportService.GetAirportByCodeAsync(airportCode));
                }
                return Ok(avAirports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving available airports of airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        public async Task<ActionResult<AirportDto>> InsertAirport([FromBody] AirportDto airportDto)
        {
            if (airportDto == null)
            {
                return BadRequest("Airport data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Airport airportNew = await _airportService.InsertAirportAsync(airportDto);
                return CreatedAtAction(nameof(InsertAirport), new { id = airportNew.Id }, airportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdateAirport(int id, [FromBody] AirportDto airportDto)
        {
            if (airportDto == null)
            {
                return BadRequest("Airport data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingAirport = await _airportService.GetAirportAsync(id);

                if (existingAirport == null)
                {
                    return NotFound();
                }

                await _airportService.UpdateAirportAsync(id, airportDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            try
            {
                var existingAirport = await _airportService.GetAirportAsync(id);

                if (existingAirport == null)
                {
                    return NotFound();
                }

                await _airportService.DeleteAirportAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting airport");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeleteAirports()
        {
            try
            {
                await _airportService.DeleteAirportsAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting airports");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

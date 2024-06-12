using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        private IPlaneService _planeService;
        private readonly ILogger<PlaneController> _logger;

        public PlaneController(IPlaneService service, ILogger<PlaneController> logger)
        {
            _planeService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<PlaneDto>>> GetPlanesDto()
        {
            try
            {
                var planes = await _planeService.GetPlanesAsync();
                return Ok(planes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving planes");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/FULL")]
        public async Task<ActionResult<IEnumerable<Plane>>> GetPlanes()
        {
            try
            {
                var planes = await _planeService.GetPlanesAsync();
                return Ok(planes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving planes");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<PlaneDto>> GetPlane(int id)
        {
            try
            {
                var planeDto = await _planeService.GetPlaneAsync(id);

                if (planeDto == null)
                {
                    return NotFound();
                }

                return Ok(planeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving plane");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        public async Task<ActionResult<PlaneDto>> InsertPlane([FromBody] PlaneDto planeDto)
        {
            if (planeDto == null)
            {
                return BadRequest("Plane data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Plane planeNew = await _planeService.InsertPlaneAsync(planeDto);
                return CreatedAtAction(nameof(InsertPlane), new { id = planeNew.Id }, planeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating plane");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdatePlane(int id, [FromBody] PlaneDto planeDto)
        {
            if (planeDto == null)
            {
                return BadRequest("Plane data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPlane = await _planeService.GetPlaneAsync(id);

                if (existingPlane == null)
                {
                    return NotFound();
                }

                await _planeService.UpdatePlaneAsync(id, planeDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating plane");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeletePlane(int id)
        {
            try
            {
                var existingPlane = await _planeService.GetPlaneAsync(id);

                if (existingPlane == null)
                {
                    return NotFound();
                }

                await _planeService.DeletePlaneAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting plane");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeletePlanes()
        {
            try
            {
                await _planeService.DeletePlanesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting planes");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MilesAccountController : ControllerBase
    {
        private IMilesAccountService _milesAccountService;
        private readonly ILogger<MilesAccountController> _logger;

        public MilesAccountController(IMilesAccountService service, ILogger<MilesAccountController> logger)
        {
            _milesAccountService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<MilesAccountDto>>> GetMilesAccounts()
        {
            try
            {
                var milesaccounts = await _milesAccountService.GetMilesAccountsAsync();
                return Ok(milesaccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving milesaccounts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<MilesAccountDto>> GetMilesAccount(int id)
        {
            try
            {
                var milesaccountDto = await _milesAccountService.GetMilesAccountAsync(id);

                if (milesaccountDto == null)
                {
                    return NotFound();
                }

                return Ok(milesaccountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving milesaccount");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        public async Task<ActionResult<MilesAccountDto>> InsertMilesAccount([FromBody] MilesAccountDto milesaccountDto)
        {
            if (milesaccountDto == null)
            {
                return BadRequest("MilesAccount data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                MilesAccount milesaccountNew = await _milesAccountService.InsertMilesAccountAsync(milesaccountDto);
                return CreatedAtAction(nameof(InsertMilesAccount), new { id = milesaccountNew.Id }, milesaccountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating milesaccount");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdateMilesAccount(int id, [FromBody] MilesAccountDto milesaccountDto)
        {
            if (milesaccountDto == null)
            {
                return BadRequest("MilesAccount data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingMilesAccount = await _milesAccountService.GetMilesAccountAsync(id);

                if (existingMilesAccount == null)
                {
                    return NotFound();
                }

                await _milesAccountService.UpdateMilesAccountAsync(id, milesaccountDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating milesaccount");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeleteMilesAccount(int id)
        {
            try
            {
                var existingMilesAccount = await _milesAccountService.GetMilesAccountAsync(id);

                if (existingMilesAccount == null)
                {
                    return NotFound();
                }

                await _milesAccountService.DeleteMilesAccountAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting milesaccount");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeleteMilesAccounts()
        {
            try
            {
                await _milesAccountService.DeleteMilesAccountsAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting milesaccounts");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Model.Enums;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService service, ILogger<TicketController> logger)
        {
            _ticketService = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GET")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            try
            {
                var tickets = await _ticketService.GetTicketsAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            try
            {
                var ticketDto = await _ticketService.GetTicketAsync(id);

                if (ticketDto == null)
                {
                    return NotFound();
                }

                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GET/USER/{userId}")]
        public async Task<ActionResult<TicketDto>> GetTicketHistoryAsync(string userId)
        {
            try
            {
                var ticketDtos = await _ticketService.GetTicketHistoryAsync(userId);

                if (ticketDtos == null)
                {
                    return NotFound();
                }

                return Ok(ticketDtos.OrderByDescending(u => u.DateCreated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("POST")]
        public async Task<ActionResult<TicketDto>> InsertTicket([FromBody] TicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("Ticket data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Ticket ticketNew = await _ticketService.InsertTicketAsync(ticketDto);
                return CreatedAtAction(nameof(InsertTicket), new { id = ticketNew.Id }, ticketDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("PUT/{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] TicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("Ticket data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingTicket = await _ticketService.GetTicketAsync(id);

                if (existingTicket == null)
                {
                    return NotFound();
                }

                await _ticketService.UpdateTicketAsync(id, ticketDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                var existingTicket = await _ticketService.GetTicketAsync(id);

                if (existingTicket == null)
                {
                    return NotFound();
                }

                await _ticketService.DeleteTicketAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DELETE")]
        public async Task<IActionResult> DeleteTickets()
        {
            try
            {
                await _ticketService.DeleteTicketsAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tickets");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

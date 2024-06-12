using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Svc;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;
        private ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }

        [HttpPost("BUY")]
        public async Task<ActionResult<TicketDto>> BuyTicket([FromBody] TicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("Airport data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var ticketNew = await _bookingService.BuyTicketAsync(ticketDto);
                return Ok(ticketNew);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buying ticket");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

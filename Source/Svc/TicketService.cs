using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class TicketService : ITicketService
    {
        private TicketAccess _ticketAccess;
        private ILogger<TicketService> _logger;

        public TicketService(TicketAccess ticketAccess, ILogger<TicketService> logger)
        {
            _ticketAccess = ticketAccess ?? throw new ArgumentNullException(nameof(ticketAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ICollection<TicketDto>> GetTicketHistoryAsync(string userId)
        {
            try
            {
                var tickets = await _ticketAccess.GetTicketsAsync();

                if (tickets == null)
                {
                    _logger.LogWarning("No tickets found");
                    return new List<TicketDto>();
                }

                var filteredTickets = tickets.AsQueryable();

                filteredTickets = filteredTickets.Where(u => u.UserId == userId);

                var ticketDtos = filteredTickets.Select(ticket => CreateTicketDto(ticket)).ToList();

                return ticketDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets");
                throw;
            }
        }

        public async Task<ICollection<TicketDto>> GetTicketsAsync()
        {
            try
            {
                var tickets = await _ticketAccess.GetTicketsAsync();
                List<TicketDto> ticketDtos = new List<TicketDto>();
                foreach (var ticket in tickets)
                {
                    ticketDtos.Add(CreateTicketDto(ticket));
                }
                return ticketDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tickets");
                throw;
            }
        }

        public async Task<TicketDto> GetTicketAsync(int Id)
        {
            try
            {
                var ticket = await _ticketAccess.GetTicketAsync(Id);
                return CreateTicketDto(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket with ID {Id}", Id);
                throw;
            }
        }

        public async Task<TicketDto> GetTicketByCodeAsync(string Code)
        {
            try
            {
                var ticket = await _ticketAccess.GetTicketByCodeAsync(Code);
                return CreateTicketDto(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ticket with ID {Code}", Code);
                throw;
            }
        }

        public async Task<Ticket> InsertTicketAsync(TicketDto ticketDto)
        {
            try
            {
                var ticket = await _ticketAccess.InsertTicketAsync(CreateTicket(ticketDto));
                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting ticket");
                throw;
            }
        }

        public async Task DeleteTicketAsync(int Id)
        {
            try
            {
                await _ticketAccess.DeleteTicketAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ticket with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeleteTicketsAsync()
        {
            try
            {
                await _ticketAccess.DeleteTicketsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tickets");
                throw;
            }
        }

        public async Task UpdateTicketAsync(int id, TicketDto ticket)
        {
            try
            { 
                Ticket ticketOld = await _ticketAccess.GetTicketAsync(id);
                if (ticketOld != null)
                {
                    ticketOld.Code = ticket.Code;
                    ticketOld.Price = ticket.Price;
                    ticketOld.DateCreated = ticket.DateCreated;
                    ticketOld.FlightCode = ticket.FlightCode;
                    ticketOld.UserId = ticket.UserId;
                    ticketOld.Status = ticket.Status;
                    ticketOld.BuyWithMilesPoints = ticket.BuyWithMilesPoints;
                    ticketOld.SeatNumbers = ticket.SeatNumbers;
                    await _ticketAccess.UpdateTicketAsync(ticketOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ticket with ID {id}", id);
                throw;
            }
        }

        private TicketDto CreateTicketDto(Ticket ticket)
        {
            TicketDto ret = new TicketDto()
            {
                Code = ticket.Code,
                SeatNumbers = ticket.SeatNumbers,
                Price = ticket.Price,
                DateCreated = ticket.DateCreated,
                FlightCode = ticket.FlightCode,
                UserId = ticket.UserId,
                BuyWithMilesPoints = ticket.BuyWithMilesPoints,
                Status = ticket.Status
            };
            return ret;
        }

        private Ticket CreateTicket(TicketDto ticketDto)
        {
            Ticket ret = new Ticket()
            {
                Code = ticketDto.Code,
                SeatNumbers = ticketDto.SeatNumbers,
                Price = ticketDto.Price,
                DateCreated = ticketDto.DateCreated,
                FlightCode = ticketDto.FlightCode,
                UserId = ticketDto.UserId,
                BuyWithMilesPoints = ticketDto.BuyWithMilesPoints,
                Status = ticketDto.Status
            };
            return ret;
        }
    }
}

using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class BookingService : IBookingService
    {
        private FlightAccess _flightAccess;
        private MilesAccountAccess _milesAccountAccess;
        private TicketAccess _ticketAccess;
        private UserAccess _userAccess;
        private ILogger<BookingService> _logger;

        private ITicketService _ticketService;

        private AirlineTicketingSystemContext _context;

        public BookingService(FlightAccess flightAccess,
                            MilesAccountAccess milesAccountAccess,
                            TicketAccess ticketAccess,
                            UserAccess userAccess,
                            ILogger<BookingService> logger,
                            ITicketService ticketService,
                            AirlineTicketingSystemContext context)
        {
            _flightAccess = flightAccess ?? throw new ArgumentNullException(nameof(flightAccess));
            _milesAccountAccess = milesAccountAccess ?? throw new ArgumentNullException(nameof(milesAccountAccess));
            _ticketAccess = ticketAccess ?? throw new ArgumentNullException(nameof(ticketAccess));
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // TODO : Can be updated as a new passenger is generated; taking passengerDto as parameter might work
        public async Task<TicketDto> BuyTicketAsync(TicketDto ticketDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Flight flight = await _flightAccess.GetFlightByCodeAsync(ticketDto.FlightCode);
                    if (flight == null)
                    {
                        _logger.LogWarning("No flights found for ticket purchase");
                        throw new ArgumentException("No flights found for ticket purchase");
                    }

                    if (ticketDto.SeatNumbers.Count == 0)
                    {
                        _logger.LogError("Seats not selected!");
                        throw new ArgumentException("Seats not selected!");
                    }

                    // Update Miles Account
                    if ( (!String.IsNullOrEmpty(ticketDto.UserId)) && (ticketDto.BuyWithMilesPoints) )
                    {
                        MilesAccount milesAccount = await _milesAccountAccess.GetMilesAccountByUserAsync(ticketDto.UserId);
                        if ((milesAccount != null) && (milesAccount.Miles >= RoundMiles(flight.TotalMiles)))
                        {
                            await _milesAccountAccess.SubtractMilesToAccountAsync(RoundMiles(flight.TotalMiles), milesAccount.Id);
                        }
                        else
                        {
                            _logger.LogWarning("Miles account not found or not enough miles points for user {UserId}", ticketDto.UserId);
                            throw new ArgumentException($"Miles account not found or not enough miles!");
                        }
                    }

                    // Update Flight Information
                    foreach (var seatNumber in ticketDto.SeatNumbers)
                    {
                        if (flight.OccupiedSeats.Contains(seatNumber))
                        {
                            _logger.LogWarning("Seat {SeatNumber} is already taken", seatNumber);
                            throw new ArgumentException($"Seat {seatNumber} is already taken");
                        }
                        else
                        {
                            flight.OccupiedSeats.Add(seatNumber);
                        }
                    }

                    flight.AvailableSeatCount -= ticketDto.SeatNumbers.Count;

                    await _flightAccess.UpdateFlightAsync(flight);
                    var ticketNew = await _ticketService.InsertTicketAsync(ticketDto);
                    ticketDto.Code = ticketNew.Id.ToString("TK-00000");
                    await _ticketService.UpdateTicketAsync(ticketNew.Id, ticketDto);

                    transaction.Commit();

                    return ticketDto;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during ticket purchase");
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private int RoundMiles(double price)
        {
            return Convert.ToInt32(Math.Round(price));
        }
    }
}

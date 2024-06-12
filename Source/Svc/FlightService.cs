using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Microsoft.Extensions.Logging;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class FlightService : IFlightService
    {
        private FlightAccess _flightAccess;
        private IDistanceCalculatorService _distanceCalculatorService;
        private IAirportService _airportService;
        private IPlaneService _planeService;
        private ILogger<FlightService> _logger;

        public FlightService(FlightAccess flightAccess, ILogger<FlightService> logger, IDistanceCalculatorService distanceCalculatorService, IAirportService airportService, IPlaneService planeService)
        {
            _flightAccess = flightAccess ?? throw new ArgumentNullException(nameof(flightAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _distanceCalculatorService = distanceCalculatorService ?? throw new ArgumentNullException(nameof(distanceCalculatorService));
            _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));
            _planeService = planeService ?? throw new ArgumentNullException(nameof(planeService));
        }

        public async Task<ICollection<FlightDto>> GetFlightsAsync()
        {
            try
            {
                var flights = await _flightAccess.GetFlightsAsync();
                List<FlightDto> flightDtos = new List<FlightDto>();
                foreach (var flight in flights)
                {
                    flightDtos.Add(CreateFlightDto(flight));
                }
                return flightDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights");
                throw;
            }
        }

        public async Task<FlightDto> GetFlightAsync(int Id)
        {
            try
            {
                var flight = await _flightAccess.GetFlightAsync(Id);
                return CreateFlightDto(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight with ID {Id}", Id);
                throw;
            }
        }

        public async Task<FlightDto> GetFlightByCodeAsync(string Code)
        {
            try
            {
                var flight = await _flightAccess.GetFlightByCodeAsync(Code);
                return CreateFlightDto(flight);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flight with ID {Code}", Code);
                throw;
            }
        }

        public async Task<ICollection<FlightDto>> GetFlightsAsync(FlightFilterModel? flightFilterModel)
        {
            try
            {
                var flights = await _flightAccess.GetFlightsAsync();
                if (flights == null)
                {
                    _logger.LogWarning("No flights found");
                    return new List<FlightDto>();
                }
                var filteredFlights = flights.AsQueryable();

                filteredFlights = filteredFlights.Where(u => (u.DepartureDate >= DateTime.Now) && (u.Status == Model.Enums.FlightStatus.Scheduled));

                if (flightFilterModel != null)
                {
                    if (!String.IsNullOrEmpty(flightFilterModel.departureAirportCode))
                        filteredFlights = filteredFlights.Where(u => (u.DepartureAirportCode == flightFilterModel.departureAirportCode) 
                                                                    || (u.DepartureAirportCode.Contains(flightFilterModel.departureAirportCode)));

                    if (!String.IsNullOrEmpty(flightFilterModel.arrivalAirportCode))
                        filteredFlights = filteredFlights.Where(u => (u.ArrivalAirportCode == flightFilterModel.arrivalAirportCode) 
                                                                    || (u.ArrivalAirportCode.Contains(flightFilterModel.arrivalAirportCode)));

                    if (flightFilterModel.departureDate.HasValue)
                        filteredFlights = filteredFlights.Where(u => u.DepartureDate.Date == flightFilterModel.departureDate.Value.Date);

                    if (flightFilterModel.arrivalDate.HasValue)
                        filteredFlights = filteredFlights.Where(u => u.ArrivalDate.Date == flightFilterModel.arrivalDate.Value.Date);

                    if (flightFilterModel.availableSeatCount.HasValue)
                        filteredFlights = filteredFlights.Where(u => u.AvailableSeatCount >= flightFilterModel.availableSeatCount.Value);
                }

                var flightDtos = filteredFlights.Select(f => CreateFlightDto(f)).ToList();

                return flightDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving flights");
                throw;
            }
        }

        public async Task<Flight> InsertFlightAsync(FlightDto flightDto)
        {
            try
            {
                var departureAirport = await _airportService.GetAirportByCodeAsync(flightDto.DepartureAirportCode) ?? throw new ArgumentNullException("Departure airport not found: " + flightDto.DepartureAirportCode);
                var arrivalAirport = await _airportService.GetAirportByCodeAsync(flightDto.ArrivalAirportCode) ?? throw new ArgumentNullException("Arrival airport not found: " + flightDto.ArrivalAirportCode);
                var plane = await _planeService.GetPlaneByCodeAsync(flightDto.PlaneCode) ?? throw new ArgumentNullException("Plane not found: " + flightDto.PlaneCode);
                flightDto.TotalMiles = await _distanceCalculatorService.CalculateAsync(
                    departureAirport.Latitude,
                    departureAirport.Longitude,
                    arrivalAirport.Latitude,
                    arrivalAirport.Longitude);
                double flightTimeHours = flightDto.TotalMiles / plane.Speed;
                double flightTimeMinutes = flightTimeHours * 60;
                flightDto.TotalMinutes = Convert.ToInt32((Math.Ceiling(flightTimeMinutes)  + 25));
                flightDto.ArrivalDate = flightDto.DepartureDate.AddMinutes(flightDto.TotalMinutes);
                flightDto.AvailableSeatCount = plane.Capacity;
                flightDto.OccupiedSeats = new List<int>();
                flightDto.DateCreated = DateTime.Now;
                var flight = await _flightAccess.InsertFlightAsync(CreateFlight(flightDto));
                return flight;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting flight");
                throw;
            }
        }

        public async Task DeleteFlightAsync(int Id)
        {
            try
            {
                await _flightAccess.DeleteFlightAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting flight with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeleteFlightsAsync()
        {
            try
            {
                await _flightAccess.DeleteFlightsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting flights");
                throw;
            }
        }

        public async Task UpdateFlightAsync(int id, FlightDto flight)
        {
            try
            {
                Flight flightOld = await _flightAccess.GetFlightAsync(id);
                if (flightOld != null)
                {
                    flightOld.DepartureDate = flight.DepartureDate;
                    flightOld.ArrivalDate = flight.ArrivalDate;
                    flightOld.DateCreated = flight.DateCreated;
                    flightOld.Status = flight.Status;
                    flightOld.DepartureAirportCode = flight.DepartureAirportCode;
                    flightOld.ArrivalAirportCode = flight.ArrivalAirportCode;
                    flightOld.PlaneCode = flight.PlaneCode;
                    flightOld.TotalMiles = flight.TotalMiles;
                    flightOld.TotalMinutes = flight.TotalMinutes;
                    flightOld.AvailableSeatCount = flight.AvailableSeatCount;
                    flightOld.OccupiedSeats = flight.OccupiedSeats;
                    await _flightAccess.UpdateFlightAsync(flightOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating flight with ID {id}", id);
                throw;
            }
        }

        private FlightDto CreateFlightDto(Flight flight)
        {
            FlightDto ret = new FlightDto()
            {
                Code = flight.Code,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                DateCreated = flight.DateCreated,
                Status = flight.Status,
                DepartureAirportCode = flight.DepartureAirportCode,
                ArrivalAirportCode = flight.ArrivalAirportCode,
                PlaneCode = flight.PlaneCode,
                TotalMiles = flight.TotalMiles,
                TotalMinutes = flight.TotalMinutes,
                AvailableSeatCount = flight.AvailableSeatCount,
                OccupiedSeats = flight.OccupiedSeats
            };
            return ret;
        }

        private Flight CreateFlight(FlightDto flightDto)
        {
            Flight ret = new Flight()
            {
                Code = flightDto.Code,
                DepartureDate = flightDto.DepartureDate,
                ArrivalDate = flightDto.ArrivalDate,
                DateCreated = flightDto.DateCreated,
                Status = flightDto.Status,
                DepartureAirportCode = flightDto.DepartureAirportCode,
                ArrivalAirportCode = flightDto.ArrivalAirportCode,
                PlaneCode = flightDto.PlaneCode,
                TotalMiles = flightDto.TotalMiles,
                TotalMinutes = flightDto.TotalMinutes,
                AvailableSeatCount = flightDto.AvailableSeatCount,
                OccupiedSeats = flightDto.OccupiedSeats
            };
            return ret;
        }
    }
}

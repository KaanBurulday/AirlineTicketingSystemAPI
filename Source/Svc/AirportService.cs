using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class AirportService : IAirportService
    {
        private AirportAccess _airportAccess;
        private ILogger<AirportService> _logger;
        private IDistanceCalculatorService _distanceCalculatorService;

        private double AvgDistanceBetweenAirports = 1000; // Miles


        public AirportService(AirportAccess airportAccess, ILogger<AirportService> logger, IDistanceCalculatorService distanceCalculatorService)
        {
            _airportAccess = airportAccess ?? throw new ArgumentNullException(nameof(airportAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _distanceCalculatorService = distanceCalculatorService ?? throw new ArgumentNullException(nameof(_distanceCalculatorService));
        }

        public async Task UpdateAvailableAirportsAsync()
        {
            try
            {
                var airports = await _airportAccess.GetAirportsAsync();
                double distance = 501;
                foreach (var airportx in airports)
                {
                    foreach (var airporty in airports)
                    {
                        if (airportx.Id == airporty.Id)
                            continue;

                        distance = await _distanceCalculatorService.CalculateBetweenAirportsAsync(airportx, airporty);
                        if (distance <= AvgDistanceBetweenAirports)
                            airportx.AvailableAirportsCodes.Add(airporty.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                throw;
            }
        }

        public async Task UpdateAvailableAirportsAsync(AirportDto airportDto)
        {
            try
            {
                var airports = await _airportAccess.GetAirportsAsync();
                double distance = AvgDistanceBetweenAirports + 1;
                foreach (var airportx in airports)
                {
                    if (airportx.Code != airportDto.Code)
                        continue;

                    distance = await _distanceCalculatorService.CalculateBetweenAirportDtosAsync(CreateAirportDto(airportx), airportDto);
                    if (distance <= AvgDistanceBetweenAirports)
                        airportDto.AvailableAirportsCodes.Add(airportx.Code);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                throw;
            }
        }

        public async Task AddPlaneToRandomAirport(string planeCode)
        {
            List<Airport> airports = (List<Airport>) await _airportAccess.GetAirportsAsync();
            Random random = new Random();
            Airport airport = airports[random.Next(airports.Count)];
            if (airport.PlaneCodes.Contains(planeCode))
            { 
                _logger.LogWarning(message: $"{planeCode} is already in the airport {airport.Code}");
            }
            else
            { 
                airport.PlaneCodes.Add(planeCode);
                await _airportAccess.UpdateAirportAsync(airport);
            }   
        }
        

        public async Task<ICollection<AirportDto>> GetAirportsAsync()
        {
            try
            {
                var airports = await _airportAccess.GetAirportsAsync();
                List<AirportDto> airportDtos = new List<AirportDto>();
                foreach (var airport in airports)
                {
                    airportDtos.Add(CreateAirportDto(airport));
                }
                return airportDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                throw;
            }
        }

        public async Task<AirportDto> GetAirportAsync(int Id)
        {
            try
            {
                var airport = await _airportAccess.GetAirportAsync(Id);
                return CreateAirportDto(airport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport with ID {Id}", Id);
                throw;
            }
        }

        public async Task<AirportDto> GetAirportByCodeAsync(string Code)
        {
            try
            {
                var airport = await _airportAccess.GetAirportByCodeAsync(Code);
                return CreateAirportDto(airport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport with ID {Code}", Code);
                throw;
            }
        }

        public async Task<Airport> InsertAirportAsync(AirportDto airportDto)
        {
            try
            {
                var airport = await _airportAccess.InsertAirportAsync(CreateAirport(airportDto));
                return airport;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting airport");
                throw;
            }
        }

        public async Task DeleteAirportAsync(int Id)
        {
            try
            {
                await _airportAccess.DeleteAirportAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting airport with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeleteAirportsAsync()
        {
            try
            {
                await _airportAccess.DeleteAirportsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting airports");
                throw;
            }
        }

        public async Task UpdateAirportAsync(int id, AirportDto airport)
        {
            try
            {
                Airport airportOld = await _airportAccess.GetAirportAsync(id);
                if (airportOld != null)
                {
                    airportOld.Code = airport.Code;
                    airportOld.Name = airport.Name;
                    airportOld.City = airport.City;
                    airportOld.Country = airport.Country;
                    airportOld.DateCreated = airport.DateCreated;
                    airportOld.Latitude = airport.Latitude;
                    airportOld.Longitude = airport.Longitude;
                    airportOld.AvailableAirportsCodes = airport.AvailableAirportsCodes;
                    airportOld.PlaneCodes = airport.PlaneCodes;
                    await _airportAccess.UpdateAirportAsync(airportOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating airport with ID {id}", id);
                throw;
            }
        }

        private AirportDto CreateAirportDto(Airport airport)
        {
            AirportDto ret = new AirportDto()
            {
                Code = airport.Code,
                Name = airport.Name,
                City = airport.City,
                Country = airport.Country,
                DateCreated = airport.DateCreated,
                Latitude = airport.Latitude,
                Longitude = airport.Longitude,
                AvailableAirportsCodes = airport.AvailableAirportsCodes,
                PlaneCodes = airport.PlaneCodes
            };
            return ret;
        }

        private Airport CreateAirport(AirportDto airportDto)
        {
            Airport ret = new Airport()
            {
                Code = airportDto.Code,
                Name = airportDto.Name,
                City = airportDto.City,
                Country = airportDto.Country,
                DateCreated = airportDto.DateCreated,
                Latitude = airportDto.Latitude,
                Longitude = airportDto.Longitude,
                AvailableAirportsCodes = airportDto.AvailableAirportsCodes,
                PlaneCodes = airportDto.PlaneCodes
            };
            return ret;
        }
    }
}

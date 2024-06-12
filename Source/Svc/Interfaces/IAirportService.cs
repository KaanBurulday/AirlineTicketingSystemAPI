using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IAirportService
    {
        public Task UpdateAvailableAirportsAsync();
        public Task AddPlaneToRandomAirport(string planeCode);


        public Task<ICollection<AirportDto>> GetAirportsAsync();
        public Task<AirportDto> GetAirportAsync(int Id);
        public Task<AirportDto> GetAirportByCodeAsync(string Code);
        public Task<Airport> InsertAirportAsync(AirportDto airportDto);
        public Task UpdateAirportAsync(int id, AirportDto airport);
        public Task DeleteAirportsAsync();
        public Task DeleteAirportAsync(int Id);
    }
}

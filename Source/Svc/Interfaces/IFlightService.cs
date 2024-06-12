using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IFlightService
    {
        public Task<ICollection<FlightDto>> GetFlightsAsync(FlightFilterModel flightFilterModel);


        public Task<ICollection<FlightDto>> GetFlightsAsync();
        public Task<FlightDto> GetFlightAsync(int Id);
        public Task<FlightDto> GetFlightByCodeAsync(string Code);
        public Task<Flight> InsertFlightAsync(FlightDto flightDto);
        public Task UpdateFlightAsync(int id, FlightDto flight);
        public Task DeleteFlightsAsync();
        public Task DeleteFlightAsync(int Id);
    }
}

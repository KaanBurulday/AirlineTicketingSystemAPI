using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IDistanceCalculatorService
    {
        public Task<double> CalculateBetweenAirportsAsync(Airport airportx, Airport airporty);     

        public Task<double> CalculateBetweenAirportDtosAsync(AirportDto airportDtox, AirportDto airportDtoy);
        public Task<double> CalculateAsync(double latx, double longx, double laty, double longy);
    }
}

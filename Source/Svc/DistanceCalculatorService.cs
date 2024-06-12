using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class DistanceCalculatorService : IDistanceCalculatorService
    {
        public async Task<double> CalculateBetweenAirportsAsync(Airport airportx, Airport airporty)
        {
            return await CalculateAsync(airportx.Latitude, airportx.Longitude, airporty.Latitude, airporty.Longitude);
        }

        public async Task<double> CalculateBetweenAirportDtosAsync(AirportDto airportDtox, AirportDto airportDtoy)
        {
            return await CalculateAsync(airportDtox.Latitude, airportDtox.Longitude, airportDtoy.Latitude, airportDtoy.Longitude);
        }

        public async Task<double> CalculateAsync(double latx, double lonx, double laty, double lony)
        {
            return await Task.Run(() =>
            {
                // Convert latitude and longitude from degrees to radians
                latx = ToRadians(latx);
                lonx = ToRadians(lonx);
                laty = ToRadians(laty);
                lony = ToRadians(lony);

                // Haversine formula
                double dlat = laty - latx;
                double dlon = lony - lonx;

                double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                           Math.Cos(latx) * Math.Cos(laty) *
                           Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                // Radius of Earth in kilometers (mean radius)
                double R = 6371.0;

                // Distance in kilometers
                double distance = R * c;

                return Math.Round(distance, 2);
            });
        }

        private double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }
    }
}

using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class FlightAccess
    {
        private AirlineTicketingSystemContext _context;

        public FlightAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlightAsync(int Id)
        {
            return await _context.Flights.FindAsync(Id);
        }

        public async Task<Flight> GetFlightByCodeAsync(string Code)
        {
            return await _context.Flights.FirstAsync(f => f.Code == Code);
        }

        public async Task<Flight> InsertFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFlightsAsync()
        {
            var flights = await _context.Flights.ToListAsync();
            if(flights.Any())
            { 
                _context.Flights.RemoveRange(flights);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlightAsync(int Id)
        {
            Flight flight = await GetFlightAsync(Id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }
    }
}

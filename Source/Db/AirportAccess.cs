using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class AirportAccess
    {
        private AirlineTicketingSystemContext _context;

        public AirportAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Airport>> GetAirportsAsync()
        {
            return await _context.Airports.ToListAsync();
        }

        public async Task<Airport> GetAirportAsync(int Id)
        {
            return await _context.Airports.FindAsync(Id);
        }

        public async Task<Airport> GetAirportByCodeAsync(string Code)
        {
            return await _context.Airports.FirstAsync(a => a.Code == Code);
        }

        public async Task<Airport> InsertAirportAsync(Airport airport)
        {
            _context.Airports.Add(airport);
            await _context.SaveChangesAsync();
            return airport;
        }

        public async Task UpdateAirportAsync(Airport airport)
        {
            _context.Entry(airport).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAirportsAsync()
        {
            var airports = await _context.Airports.ToListAsync();
            if(airports.Any())
            {
                _context.Airports.RemoveRange(airports);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAirportAsync(int Id)
        {
            Airport airport = await GetAirportAsync(Id);
            if (airport != null)
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class PlaneAccess
    {
        private AirlineTicketingSystemContext _context;

        public PlaneAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task<IEnumerable<Plane>> GetPlanesAsync()
        {
            return await _context.Planes.ToListAsync();
        }

        public async Task<Plane> GetPlaneAsync(int Id)
        {
            return await _context.Planes.FindAsync(Id);
        }

        public async Task<Plane> GetPlaneByCodeAsync(string Code)
        {
            return await _context.Planes.FirstAsync(p => p.Code == Code);
        }

        public async Task<Plane> InsertPlaneAsync(Plane plane)
        {
            _context.Planes.Add(plane);
            await _context.SaveChangesAsync();
            return plane;
        }

        public async Task UpdatePlaneAsync(Plane plane)
        {
            _context.Entry(plane).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlanesAsync()
        {
            var planes = await _context.Planes.ToListAsync();
            if (planes.Any())
            {
                _context.Planes.RemoveRange(planes);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePlaneAsync(int Id)
        {
            Plane plane = await GetPlaneAsync(Id);
            if (plane != null)
            {
                _context.Planes.Remove(plane);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Context
{
    public class AirlineTicketingSystemContext : DbContext
    {
        public AirlineTicketingSystemContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<MilesAccount> MilesAccounts { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

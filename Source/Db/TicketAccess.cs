using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class TicketAccess
    {
        private AirlineTicketingSystemContext _context;

        public TicketAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetTicketAsync(int Id)
        {
            return await _context.Tickets.FindAsync(Id);
        }

        public async Task<Ticket> GetTicketByCodeAsync(string Code)
        {
            return await _context.Tickets.FirstAsync(t => t.Code == Code);
        }

        public async Task<Ticket> InsertTicketAsync(Ticket ticket)
        {
            ticket.Code = ticket.Id.ToString("TK-00000");
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketsAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();
            if (tickets.Any())
            {
                _context.Tickets.RemoveRange(tickets);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTicketAsync(int Id)
        {
            Ticket ticket = await GetTicketAsync(Id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}

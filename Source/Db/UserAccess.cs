using AirlineTicketingSystemAPI.Context;
using AirlineTicketingSystemAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class UserAccess
    {
        private AirlineTicketingSystemContext _context;

        public UserAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstAsync(user => user.Email == email);
        }

        public async Task<User> InsertUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Any())
            {
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int Id)
        {
            User user = await GetUserAsync(Id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

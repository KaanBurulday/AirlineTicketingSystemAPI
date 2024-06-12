using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace AirlineTicketingSystemAPI.Source.Db
{
    public class MilesAccountAccess
    {
        private AirlineTicketingSystemContext _context;

        public MilesAccountAccess(AirlineTicketingSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task AddMilesToAccountAsync(int miles, int accountId)
        {
            MilesAccount milesAccount = await GetMilesAccountAsync(accountId);
            if (milesAccount != null)
            {
                milesAccount.Miles += miles;
                await UpdateMilesAccountAsync(milesAccount);
            }
        }

        public async Task SubtractMilesToAccountAsync(int miles, int accountId)
        {
            MilesAccount milesAccount = await GetMilesAccountAsync(accountId);
            if ( (milesAccount != null) && (milesAccount.Miles >= miles) )
            {
                milesAccount.Miles -= miles;
                await UpdateMilesAccountAsync(milesAccount);
            }
        }

        public async Task<IEnumerable<MilesAccount>> GetMilesAccountsAsync()
        {
            return await _context.MilesAccounts.ToListAsync();
        }

        public async Task<MilesAccount> GetMilesAccountAsync(int Id)
        {
            return await _context.MilesAccounts.FindAsync(Id);
        }

        public async Task<MilesAccount> GetMilesAccountByUserAsync(string userId)
        {
            return await _context.MilesAccounts.FirstAsync(m => m.UserId == userId);
        }

        public async Task<MilesAccount> InsertMilesAccountAsync(MilesAccount milesAccount)
        {
            _context.MilesAccounts.Add(milesAccount);
            await _context.SaveChangesAsync();
            return milesAccount;
        }

        public async Task UpdateMilesAccountAsync(MilesAccount milesAccount)
        {
            _context.Entry(milesAccount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMilesAccountsAsync()
        {
            var milesAccounts = await _context.MilesAccounts.ToListAsync();
            if (milesAccounts.Any())
            {
                _context.MilesAccounts.RemoveRange(milesAccounts);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMilesAccountAsync(int Id)
        {
            MilesAccount milesAccount = await GetMilesAccountAsync(Id);
            if (milesAccount != null)
            {
                _context.MilesAccounts.Remove(milesAccount);
                await _context.SaveChangesAsync();
            }
        }
    }
}

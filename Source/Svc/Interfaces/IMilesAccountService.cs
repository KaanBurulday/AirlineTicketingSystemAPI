using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IMilesAccountService
    {
        public Task<ICollection<MilesAccountDto>> GetMilesAccountsAsync();
        public Task<MilesAccountDto> GetMilesAccountAsync(int Id);
        public Task<MilesAccountDto> GetMilesAccountByUserAsync(string userId);
        public Task<MilesAccount> InsertMilesAccountAsync(MilesAccountDto milesAccountDto);
        public Task UpdateMilesAccountAsync(int id, MilesAccountDto milesAccount);
        public Task DeleteMilesAccountsAsync();
        public Task DeleteMilesAccountAsync(int Id); 
    }
}

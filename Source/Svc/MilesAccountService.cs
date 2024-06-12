using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class MilesAccountService : IMilesAccountService
    {
        private MilesAccountAccess _milesAccountAccess;
        private ILogger<MilesAccountService> _logger;

        public MilesAccountService(MilesAccountAccess milesAccountAccess, ILogger<MilesAccountService> logger)
        {
            _milesAccountAccess = milesAccountAccess ?? throw new ArgumentNullException(nameof(milesAccountAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ICollection<MilesAccountDto>> GetMilesAccountsAsync()
        {
            try
            {
                var milesAccounts = await _milesAccountAccess.GetMilesAccountsAsync();
                List<MilesAccountDto> milesAccountDtos = new List<MilesAccountDto>();
                foreach (var milesAccount in milesAccounts)
                {
                    milesAccountDtos.Add(CreateMilesAccountDto(milesAccount));
                }
                return milesAccountDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving miles accounts");
                throw;
            }
        }

        public async Task<MilesAccountDto> GetMilesAccountAsync(int Id)
        {
            try
            {
                var milesAccount = await _milesAccountAccess.GetMilesAccountAsync(Id);
                return CreateMilesAccountDto(milesAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving miles account with ID {Id}", Id);
                throw;
            }
        }

        public async Task<MilesAccountDto> GetMilesAccountByUserAsync(string userId)
        {
            try
            {
                var milesAccount = await _milesAccountAccess.GetMilesAccountByUserAsync(userId);
                return CreateMilesAccountDto(milesAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving miles account with ID {userId}", userId);
                throw;
            }
        }

        public async Task<MilesAccount> InsertMilesAccountAsync(MilesAccountDto milesAccountDto)
        {
            try
            {
                var milesAccount = await _milesAccountAccess.InsertMilesAccountAsync(CreateMilesAccount(milesAccountDto));
                return milesAccount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting miles account");
                throw;
            }
        }

        public async Task DeleteMilesAccountAsync(int Id)
        {
            try
            {
                await _milesAccountAccess.DeleteMilesAccountAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting miles account with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeleteMilesAccountsAsync()
        {
            try
            {
                await _milesAccountAccess.DeleteMilesAccountsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting miles accounts");
                throw;
            }
        }

        public async Task UpdateMilesAccountAsync(int id, MilesAccountDto milesaccount)
        {
            try
            {
                MilesAccount milesaccountOld = await _milesAccountAccess.GetMilesAccountAsync(id);
                if (milesaccountOld != null)
                {
                    milesaccountOld.Miles = milesaccount.Miles;
                    await _milesAccountAccess.UpdateMilesAccountAsync(milesaccountOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating miles account with ID {id}", id);
                throw;
            }
        }

        private MilesAccountDto CreateMilesAccountDto(MilesAccount milesAccount)
        {
            MilesAccountDto ret = new MilesAccountDto()
            {
                Miles = milesAccount.Miles
            };
            return ret;
        }

        private MilesAccount CreateMilesAccount(MilesAccountDto milesAccountDto)
        {
            MilesAccount ret = new MilesAccount()
            {
                Miles = milesAccountDto.Miles
            };
            return ret;
        }
    }
}

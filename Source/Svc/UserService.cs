using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class UserService : IUserService
    {
        private UserAccess _userAccess;
        private ILogger<UserService> _logger;

        public UserService(UserAccess userAccess, ILogger<UserService> logger)
        {
            _userAccess = userAccess ?? throw new ArgumentNullException(nameof(userAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _userAccess.GetUserByEmailAsync(email);
            if (user == null || user.Password != password)
            {
                return null;
            }
            return user;
        }

        public async Task<ICollection<UserDto>> GetUsersAsync()
        {
            try
            {
                var users = await _userAccess.GetUsersAsync();
                List<UserDto> userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    userDtos.Add(CreateUserDto(user));
                }
                return userDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                throw;
            }
        }

        public async Task<UserDto> GetUserAsync(int Id)
        {
            try
            {
                var user = await _userAccess.GetUserAsync(Id);
                return CreateUserDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {Id}", Id);
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string Email)
        {
            try
            {
                var user = await _userAccess.GetUserByEmailAsync(Email);
                return CreateUserDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with email {Email}", Email);
                throw;
            }
        }

        public async Task<User> InsertUserAsync(UserDto userDto)
        {
            try
            {
                var user = await _userAccess.InsertUserAsync(CreateUser(userDto));
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting user");
                throw;
            }
        }

        public async Task DeleteUserAsync(int Id)
        {
            try
            {
                await _userAccess.DeleteUserAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeleteUsersAsync()
        {
            try
            {
                await _userAccess.DeleteUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting users");
                throw;
            }
        }

        public async Task UpdateUserAsync(int id, UserDto user)
        {
            try
            { 
                User userOld = await _userAccess.GetUserAsync(id);
                if (userOld != null)
                {
                    userOld.Email = user.Email;
                    userOld.Password = user.Password;
                    userOld.DateCreated = user.DateCreated;
                    userOld.UserRole = user.UserRole;
                    await _userAccess.UpdateUserAsync(userOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {id}", id);
                throw;
            }
        }

        private UserDto CreateUserDto(User user)
        {
            UserDto ret = new UserDto()
            {
                Email = user.Email,
                Password = user.Password,
                DateCreated = user.DateCreated,
                UserRole = user.UserRole
            };
            return ret;
        }

        private User CreateUser(UserDto userDto)
        {
            User ret = new User()
            {
                Email = userDto.Email,
                Password = userDto.Password,
                DateCreated = userDto.DateCreated,
                UserRole = userDto.UserRole
            };
            return ret;
        }
    }
}

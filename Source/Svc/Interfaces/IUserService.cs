using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IUserService
    {
        public Task<User> AuthenticateUser(string email, string password);

        public Task<ICollection<UserDto>> GetUsersAsync();
        public Task<UserDto> GetUserAsync(int Id);
        public Task<UserDto> GetUserByEmailAsync(string Email);
        public Task<User> InsertUserAsync(UserDto userDto);
        public Task UpdateUserAsync(int id, UserDto user);
        public Task DeleteUsersAsync();
        public Task DeleteUserAsync(int Id);
    }
}

using AirlineTicketingSystemAPI.Model.Enums;

namespace AirlineTicketingSystemAPI.Model.Dto
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

using AirlineTicketingSystemAPI.Model;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}

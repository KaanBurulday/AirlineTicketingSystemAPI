using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IBookingService
    {
        public Task<TicketDto> BuyTicketAsync(TicketDto ticketDto);
    }
}

using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface ITicketService
    {
        public Task<ICollection<TicketDto>> GetTicketHistoryAsync(string userId); 
        public Task<ICollection<TicketDto>> GetTicketsAsync();
        public Task<TicketDto> GetTicketAsync(int Id);
        public Task<TicketDto> GetTicketByCodeAsync(string Code);
        public Task<Ticket> InsertTicketAsync(TicketDto ticketDto);
        public Task UpdateTicketAsync(int id, TicketDto ticket);
        public Task DeleteTicketsAsync();
        public Task DeleteTicketAsync(int Id);
    }
}

using AirlineTicketingSystemAPI.Model.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AirlineTicketingSystemAPI.Model.Dto
{
    public class TicketDto
    {
        public string Code { get; set; }
        public double Price { get; set; }
        public DateTime DateCreated { get; set; }
        public TicketStatus Status { get; set; }
        public string FlightCode { get; set; }
        public bool BuyWithMilesPoints { get; set; }

        public List<int> SeatNumbers { get; set; } = new List<int>();

        public string? UserId { get; set; }
    }
}

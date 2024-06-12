using AirlineTicketingSystemAPI.Model.Enums;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineTicketingSystemAPI.Model
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        [DefaultValue(TicketStatus.Booked)]
        public TicketStatus Status { get; set; }
        [Required]
        public string FlightCode { get; set; }
        [Required]
        public bool BuyWithMilesPoints { get; set; }

        public string? UserId { get; set; }

        public List<int> SeatNumbers { get; set; } = new List<int>();
    }
}

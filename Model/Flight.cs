using AirlineTicketingSystemAPI.Model.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineTicketingSystemAPI.Model
{
    [Table("Flight")]
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime DepartureDate { get; set; }
        [Required]
        public DateTime ArrivalDate { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        [DefaultValue(FlightStatus.Scheduled)]
        public FlightStatus Status { get; set; }
        [Required]
        public double TotalMiles { get; set; }
        [Required]
        public int TotalMinutes { get; set; }
        [Required]
        public int AvailableSeatCount { get; set; }

        [Required]
        public string DepartureAirportCode { get; set; }
        [Required]
        public string ArrivalAirportCode { get; set; }
        [Required]
        public string PlaneCode { get; set; }
        public List<int> OccupiedSeats { get; set; } = new List<int>();
    }
}

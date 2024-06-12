using AirlineTicketingSystemAPI.Model.Enums;
using UniversityTuitionPaymentV2.Model.Constants;

namespace AirlineTicketingSystemAPI.Model.Dto
{
    public class FlightDto
    {
        public string Code { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureAirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string PlaneCode { get; set; }
        public FlightStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public double TotalMiles { get; set; }
        public int TotalMinutes { get; set; }
        public int AvailableSeatCount { get; set; }
        public List<int> OccupiedSeats { get; set; } = new List<int>();
    }

    public class FlightFilterModel
    {
        public string? departureAirportCode { get; set; }
        public string? arrivalAirportCode { get; set; }
        public DateTime? departureDate { get; set; }
        public DateTime? arrivalDate { get; set; }
        public int? availableSeatCount { get; set; }
    }
}

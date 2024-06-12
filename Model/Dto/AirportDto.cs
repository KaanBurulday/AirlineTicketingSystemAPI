namespace AirlineTicketingSystemAPI.Model.Dto
{
    public class AirportDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime DateCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> AvailableAirportsCodes { get; set; } = new List<string>();
        public List<string> PlaneCodes { get; set; } = new List<string>();
    }
}

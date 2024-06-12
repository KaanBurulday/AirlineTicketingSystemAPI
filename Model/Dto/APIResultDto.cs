namespace AirlineTicketingSystemAPI.Model.Dto
{
    public class APIResultDto
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public APIResultDto()
        {
            Status = "SUCCESS";
        }
    }
}

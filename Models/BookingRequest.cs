namespace YachtCareAPI.Models
{
    public class BookingRequest
    {
        public string ClientName { get; set; }
        public string ServiceType { get; set; }
        public DateTime PreferredDate { get; set; }
    }
}

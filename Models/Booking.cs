namespace YachtCareAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ServiceType { get; set; }
        public DateTime PreferredDate { get; set; }
        public string Status { get; set; } // рассмотрение, подтерждение, отмена
    }
}

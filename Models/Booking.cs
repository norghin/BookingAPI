namespace YachtCareAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public DateTime PreferredDate { get; set; }
        public string Status { get; set; } = "Pending";

        public List<PriceItem> PriceList { get; set; } = new();
        public decimal TotalPrice => PriceList.Sum(p => p.Price);
    }
}

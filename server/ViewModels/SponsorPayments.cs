namespace SponsorshipBackend.ViewModels
{
    public class SponsorPayments
    {
        public string? SponsorName { get; set; }
        public string? IndustryType { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
        public decimal TotalPayment { get; set; }
        public int NoOfPayment { get; set; }
        public string? LatestPayment { get; set; }
    }
}

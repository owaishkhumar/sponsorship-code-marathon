using System.ComponentModel.DataAnnotations;

namespace SponsorshipBackend.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "Provide Contract Id")]
        public int ContractId { get; set; }
        [Required(ErrorMessage = "Provide Payment Date")]
        public string? PaymentDate { get; set; }
        [Required(ErrorMessage = "Provide Amount that paid")]
        public decimal AmountPaid { get; set; }
        public string? PaymentStatus { get; set; }

    }
}

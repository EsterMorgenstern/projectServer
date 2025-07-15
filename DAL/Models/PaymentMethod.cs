namespace DAL.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public int StudentId { get; set; }
        public string? MethodType { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? LastFourDigits { get; set; }
        public string? CardType { get; set; }
        public int? ExpiryMonth { get; set; }
        public int? ExpiryYear { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
        // Navigation Properties
        public virtual Student? Student { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}

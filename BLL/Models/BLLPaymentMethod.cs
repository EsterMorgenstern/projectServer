using DAL.Models;

public class BLLPaymentMethod
{
    public int PaymentMethodId { get; set; }
    public int StudentId { get; set; }
    public string? MethodType { get; set; } // CREDIT_CARD, BANK_TRANSFER, CASH, CHECK
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // פרטים מוצפנים
    public string? LastFourDigits { get; set; }
    public string? CardType { get; set; }
    public int? ExpiryMonth { get; set; }
    public int? ExpiryYear { get; set; }
    public string? BankName { get; set; }
    public string? AccountHolderName { get; set; }

    
}
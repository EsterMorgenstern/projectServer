public class BLLPayment
{
    public int PaymentId { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    public int? PaymentMethodId { get; set; }
    public string? Status { get; set; } // PENDING, COMPLETED, FAILED, CANCELLED
    public string? TransactionId { get; set; }
    public int? GroupId { get; set; }
    public DateTime CreatedAt { get; set; }


}


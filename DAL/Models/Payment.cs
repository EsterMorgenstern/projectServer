using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? StudentId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public virtual Student? Student { get; set; }
}

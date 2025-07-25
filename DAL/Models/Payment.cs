﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Payment
{
    public int PaymentId { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    public int? PaymentMethodId { get; set; }
    public string? Status { get; set; }
    public string? TransactionId { get; set; }
    public int? GroupId { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Student? Student { get; set; }
    public virtual PaymentMethod? PaymentMethodDetails { get; set; }
    public virtual Group? Group { get; set; }
}

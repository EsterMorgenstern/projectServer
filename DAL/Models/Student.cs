using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Age { get; set; }

    public string? City { get; set; }

    public string? School { get; set; }

    public string? HealthFund { get; set; }

    public string? Class { get; set; }

    public string? Sector { get; set; }

    public DateOnly? LastActivityDate { get; set; }
    public string? Status { get; set; }
    public string? Email { get; set; }


    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public virtual ICollection<StudentNote> StudentNotes { get; set; } = new List<StudentNote>();
}

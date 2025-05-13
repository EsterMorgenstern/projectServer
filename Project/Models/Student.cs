using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string? City { get; set; }

    public string? School { get; set; }

    public string? HealthFund { get; set; }

    public string? Gender { get; set; }

    public string? Sector { get; set; }

    public DateOnly? LastActivityDate { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

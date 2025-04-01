using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DAL.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }

    public string? City { get; set; }

    public string? School { get; set; }
    public string? HealthFund { get; set; }


}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class Student
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [StringLength(20)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    [StringLength(20)]
    public string? City { get; set; }

    [StringLength(20)]
    public string? School { get; set; }

    [StringLength(50)]
    public string? HealthFund { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}

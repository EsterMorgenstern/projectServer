using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class Instructor
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [StringLength(20)]
    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    [StringLength(30)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? City { get; set; }

    [InverseProperty("Instructor")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}

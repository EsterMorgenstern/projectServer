using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class GroupStudent
{
    public int GroupStudentId { get; set; }

    public int StudentId { get; set; }

    public int GroupId { get; set; }

    public DateOnly? EnrollmentDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

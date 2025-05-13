using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public int BranchId { get; set; }

    public int CourseId { get; set; }

    public int InstructorId { get; set; }

    public string GroupName { get; set; } = null!;

    public int DayOfWeek { get; set; }

    public TimeOnly? Hour { get; set; }

    public string? AgeRange { get; set; }

    public int? MaxStudents { get; set; }

    public string? City { get; set; }

    public string? Sector { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual Instructor Instructor { get; set; } = null!;
}

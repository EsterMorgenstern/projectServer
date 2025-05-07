using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CouresName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}

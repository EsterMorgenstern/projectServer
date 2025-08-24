using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Branch
{
    public int BranchId { get; set; }
    public int CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public int? MaxGroupSize { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    public virtual Course Course { get; set; } = null!;

}

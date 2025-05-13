using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public int? MaxGroupSize { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}

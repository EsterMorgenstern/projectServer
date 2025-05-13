using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Instructor
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? City { get; set; }

    public string? Sector { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}

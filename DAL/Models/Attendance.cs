using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int? GroupId { get; set; }

    public int? StudentId { get; set; }

    public DateOnly? Date { get; set; }

    public bool? WasPresent { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Student? Student { get; set; }
}

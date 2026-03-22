namespace DAL.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }
    public int LessonId { get; set; }
    public int StudentId { get; set; }
    public bool WasPresent { get; set; }
    public byte StatusReport { get; set; } // 1=דווח, 2=לא לדיווח, 3=ממתין לדיווח
    public int? HealthFundReport { get; set; }
    public DateOnly? DateReport { get; set; }
    public DateTime UpdateDate { get; set; }
    public int? UpdateBy { get; set; }

    // Navigation properties
    public virtual Lesson? Lesson { get; set; }
    public virtual Student? Student { get; set; }
    public virtual HealthFund? HealthFundReportNavigation { get; set; }
}


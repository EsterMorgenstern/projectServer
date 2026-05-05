namespace DAL.Models;

public partial class GroupStudent
{
    public int GroupStudentId { get; set; }
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public DateOnly? EnrollmentDate { get; set; }
    public DateOnly? TrialDate { get; set; }
    public byte? IsActive { get; set; }  // פעיל עזב ליד ניסיון
    public virtual Group Group { get; set; } = null!;
    public virtual Student Student { get; set; } = null!;
}

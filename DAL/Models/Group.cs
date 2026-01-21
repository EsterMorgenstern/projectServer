namespace DAL.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public int BranchId { get; set; }

    public int CourseId { get; set; }

    public int InstructorId { get; set; }

    public string GroupName { get; set; } = null!;

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly? Hour { get; set; }

    public string? AgeRange { get; set; }

    public int? MaxStudents { get; set; }

    public string? Sector { get; set; }

    public DateOnly? StartDate { get; set; }

    public int? NumOfLessons { get; set; }

    public int? LessonsCompleted { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<GroupStudent> GroupStudents { get; set; } = new List<GroupStudent>();

    public virtual Instructor Instructor { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Lesson> Lessons { get; set; } = null!;



}

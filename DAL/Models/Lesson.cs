namespace DAL.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int GroupId { get; set; }
        public DateOnly LessonDate { get; set; }
        public TimeOnly? LessonHour { get; set; }
        public int? InstructorId { get; set; }
        public string Status { get; set; } = "future";
        public bool IsReported { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public virtual Group Group { get; set; } = null!;
        public virtual Instructor Instructor { get; set; } = null!;


    }
}

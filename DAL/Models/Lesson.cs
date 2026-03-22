namespace DAL.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public int GroupId { get; set; }
        public DateOnly LessonDate { get; set; }
        public TimeOnly? LessonHour { get; set; }
        public int? InstructorId { get; set; }
        public string Status { get; set; } = "future";   // "future", "done", "canceled", "completion"

        // שדות חדשים לביטול
        public string? CancellationReason { get; set; }  // nullable
        public DateTime? CanceledAt { get; set; }        // nullable
        public string? CanceledBy { get; set; }          // nullable

        public bool IsReported { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public virtual Group Group { get; set; } = null!;
        public virtual Instructor Instructor { get; set; } = null!;
        public virtual ICollection<Attendance>? Attendances { get; set; }

    }
}

namespace BLL.Models
{
    public class LessonCalendarItemDto
    {
        public int LessonId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateOnly LessonDate { get; set; }
        public TimeOnly? LessonHour { get; set; }
        public int? InstructorId { get; set; }
        public string LessonStatus { get; set; } = "future";
        public string? CancellationReason { get; set; }
    }
}
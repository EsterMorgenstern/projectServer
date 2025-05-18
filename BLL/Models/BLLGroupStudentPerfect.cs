namespace BLL.Models
{
    public class BLLGroupStudentPerfect
    {
        public int StudentId { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public bool? IsActive { get; set; }
        public int DayOfWeek { get; set; }
        public TimeOnly? Hour { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;

    }
}

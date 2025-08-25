using DAL.Models;

namespace BLL.Models
{
    public class BLLGroupStudentPerfect
    {
        public int GroupStudentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } =string.Empty;
        public required Student Student { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public bool? IsActive { get; set; }
        public string DayOfWeek { get; set; } = null!;
        public TimeOnly? Hour { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;

    }
}

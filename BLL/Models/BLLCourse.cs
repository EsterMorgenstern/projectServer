using BLL.Api;

namespace BLL.Models
{
    public class BLLCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty; // Initialize with a default value
        public int InstructorId { get; set; }
        public int NumOfStudents { get; set; }
        public int MaxNumOfStudent { get; set; }
        public DateTime StartDate { get; set; }
        public string Branch { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
    }
}

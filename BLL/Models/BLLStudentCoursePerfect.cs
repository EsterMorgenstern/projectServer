namespace BLL.Models
{
    public class BLLStudentCoursePerfect
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int InstructorId { get; set; } 
        public DateTime RegistrationDate { get; set; }

    }
}

namespace BLL.Models
{
    public class BLLCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int InstructorId { get; set; }
        public int NumOfStudents { get; set; }
        public int MaxNumOfStudent { get; set; }

        public DateTime StartDate { get; set; }

    }
}

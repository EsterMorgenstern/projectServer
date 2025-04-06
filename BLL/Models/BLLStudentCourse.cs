namespace BLL.Models
{
    public class BLLStudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public BLLStudentCourse() { }

        public BLLStudentCourse(int studentId, int courseId, DateTime registrationDate)
        {
            StudentId = studentId;
            CourseId = courseId;
            RegistrationDate = registrationDate;

        }
    }
}

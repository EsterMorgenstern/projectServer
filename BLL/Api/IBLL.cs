namespace BLL.Api
{
    public interface IBLL
    {
        public IBLLStudent Students { get; }
        public IBLLInstructor Instructors { get; }
        public IBLLCourse Courses { get; }
        public IBLLStudentCourse StudentCourses { get; }

    }
}

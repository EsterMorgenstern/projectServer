namespace BLL.Api
{
    public interface IBLL
    {
        public IBLLCourse Students { get; }
        public IBLLInstructor Instructors { get; }
        public IBLLCourse Courses { get; }
        public IBLLStudentCourse StudentCourses { get; }

    }
}

namespace BLL.Api
{
    public interface IBLL
    {
        public IBLLStudent Students { get; }
        public IBLLInstructor Instructors { get; }
        public IBLLCourse Courses { get; }
        public IBLLGroup Groups { get; }
        public IBLLAttendance Attendances { get; }
        public IBLLBranch Branches { get; }
        public IBLLGroupStudent GroupStudents { get; }
        public IBLLStudentNote Notes { get; }  
        public IBLLUser Users { get; }
        
    }
}

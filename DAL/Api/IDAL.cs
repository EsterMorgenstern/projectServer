namespace DAL.Api
{
    public interface IDAL
    {
        public IDALStudent Students { get; }
        public IDALInstructor Instructors { get; }
        public IDALCourse Courses { get; }
        public IDALGroup Groups { get; }
        public IDALAttendance Attendances { get; }
        public IDALGroupStudent GroupStudents { get; }
        public IDALBranch Branches { get; }
        public IDALStudentNote StudentNotes { get; }
        public IDALUser Users { get; }
        public IDALLessonCancellations LessonCancellations { get; }
        public IDALPaymentMethod PaymentMethods { get; }
        public IDALPayment Payments { get; }
    }
}

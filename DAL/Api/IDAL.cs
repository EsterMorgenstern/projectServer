namespace DAL.Api
{
    public interface IDAL
    {
        IDALStudent Students { get; }
        IDALInstructor Instructors { get; }
        IDALCourse Courses { get; }
        IDALGroup Groups { get; }
        IDALAttendance Attendances { get; }
        IDALGroupStudent GroupStudents { get; }
        IDALBranch Branches { get; }
        IDALStudentNote StudentNotes { get; }
        IDALUser Users { get; }
        IDALLessonCancellations LessonCancellations { get; }
        IDALPaymentMethod PaymentMethods { get; }
        IDALPayment Payments { get; }
        IDALGrowPayment PaymentGrow { get; }
        IDALHealthFund HealthFunds { get; }
        IDALStudentHealthFund StudentHealthFunds { get; }
        IDALReportedDate ReportedDates { get; } 
        IDALUnreportedDate UnreportedDates { get; }   
    }
}

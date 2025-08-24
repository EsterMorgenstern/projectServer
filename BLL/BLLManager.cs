using BLL.Api;
using BLL.Services;
using DAL.Api;

namespace BLL
{
    public class BLLManager : IBLL
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
        public IBLLLessonCancellations LessonCancellations { get; }
        public IBLLPaymentMethod PaymentMethods { get; }
        public IBLLPayment Payments { get; }

        public BLLManager(IDAL dal)
        {
            Students = new BLLStudentService(dal);
            Instructors = new BLLInstructorService(dal);
            Courses = new BLLCourseService(dal);
            Groups = new BLLGroupService(dal);
            Attendances = new BLLAttendanceService(dal);
            Branches = new BLLBranchService(dal);
            GroupStudents = new BLLGroupStudentService(dal);
            Notes = new BLLStudentNoteService(dal);
            Users = new BLLUserService(dal);
            LessonCancellations = new BLLLessonCancellationsService(dal);
            PaymentMethods = new BLLPaymentMethodService(dal);
            Payments = new BLLPaymentService(dal);
        }
    }
}

using System.Configuration;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using System.Net.Http;

namespace DAL
{
    public class DALManager : IDAL
    {
        private readonly dbcontext data = new dbcontext();

        public DALManager(IHttpClientFactory httpClientFactory)
        {
            var httpClient = httpClientFactory.CreateClient(); // יצירת HttpClient באמצעות IHttpClientFactory
            Students = new DALStudentService(data);
            Instructors = new DALInstructorService(data);
            Courses = new DALCourseService(data);
            Groups = new DALGroupService(data);
            GroupStudents = new DALGroupStudentService(data);
            Branches = new DALBranchService(data);
            Attendances = new DALAttendanceService(data);
            StudentNotes = new DALStudentNoteService(data);
            Users = new DALUserService(data);
            LessonCancellations = new DALLessonCancellationsService(data);
            PaymentMethods = new DALPaymentMethodService(data);
            Payments = new DALPaymentService(data);
            PaymentGrow = new DALGrowPaymentService(httpClient); // העברת HttpClient ל-DALGrowPaymentService
        }

        public IDALStudent Students { get; }
        public IDALInstructor Instructors { get; }
        public IDALCourse Courses { get; }
        public IDALGroup Groups { get; }
        public IDALGroupStudent GroupStudents { get; }
        public IDALBranch Branches { get; }
        public IDALAttendance Attendances { get; }
        public IDALStudentNote StudentNotes { get; }
        public IDALUser Users { get; }
        public IDALLessonCancellations LessonCancellations { get; }
        public IDALPaymentMethod PaymentMethods { get; }
        public IDALPayment Payments { get; }
        public IDALGrowPayment PaymentGrow { get; }
    }
}

using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLStudentCourseService : IBLLStudentCourse
    {
        IDAL dal;
        public BLLStudentCourseService(IDAL dal)
        {
            this.dal = dal;
        }
        public void Create(BLLStudentCourse studentCourses)
        {
            StudentCourse p = new StudentCourse()
            {
                CourseId = studentCourses.CourseId,
                StudentId = studentCourses.StudentId,
                RegistrationDate = studentCourses.RegistrationDate
            };
            dal.StudentCourses.Create(p);
        }

        public List<BLLStudentCourse> Get()
        {
            // Assuming we need to fetch the student courses from the DAL layer
            var studentCourses = dal.StudentCourses.Get();
            return studentCourses.Select(sc => new BLLStudentCourse
            {
                StudentId = sc.StudentId,
                CourseId = sc.CourseId,
                RegistrationDate = sc.RegistrationDate
            }).ToList();
        }
        public BLLStudentCourse GetById(int id)
        {
            var p = dal.StudentCourses.GetById(id);
            if (p == null)
            {
                throw new KeyNotFoundException($"StudentCourse with id {id} not found.");
            }
            return new BLLStudentCourse
            {
                StudentId = p.StudentId,
                CourseId = p.CourseId,
                RegistrationDate = p.RegistrationDate
            };
        }
        public void Delete(BLLStudentCourse studentCourse)
        {
            var m = dal.StudentCourses.GetById(studentCourse.CourseId);
            if (m == null)
            {
                throw new KeyNotFoundException($"StudentCourse with id {studentCourse.CourseId} not found.");
            }
            //we want to delete all of the students in the course

            //

            //

            dal.StudentCourses.Delete(m);
        }


        public void Update(BLLStudentCourse studentCourse)
        {
            var m = dal.StudentCourses.GetById(studentCourse.CourseId);
            m.StudentId = studentCourse.StudentId;
            m.CourseId = studentCourse.CourseId;
            m.RegistrationDate = studentCourse.RegistrationDate;

            dal.StudentCourses.Update(m);
        }
    }
}




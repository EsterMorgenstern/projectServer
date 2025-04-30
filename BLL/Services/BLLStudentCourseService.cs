using System.Collections.Generic;
using System.Security.Cryptography;
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
            var course = dal.Courses.GetById(studentCourses.CourseId);
            if (course != null)
            {
                course.NumOfStudents += 1;
                dal.Courses.Update(course);
            }
        }

        public List<BLLStudentCourse> Get()
        {

            var studentCourses = dal.StudentCourses.Get();
            return studentCourses.Select(sc => new BLLStudentCourse
            {
                StudentId = sc.StudentId,
                CourseId = sc.CourseId,
                RegistrationDate = sc.RegistrationDate ?? DateTime.MinValue // Handle nullable DateTime
            }).ToList();
        }
        public BLLStudentCourse GetById(int cId, int sId)
        {
            var p = dal.StudentCourses.GetById(cId, sId);
            if (p == null)
            {
                throw new KeyNotFoundException($"StudentCourse with courseId {cId} & studentId {sId} not found.");
            }
            return new BLLStudentCourse
            {
                StudentId = p.StudentId,
                CourseId = p.CourseId,
                RegistrationDate = p.RegistrationDate ?? DateTime.MinValue // Handle nullable DateTime
            };
        }
        public BLLStudentCourse GetByIdCourse(int cId)
        {
            var p = dal.StudentCourses.GetByIdCourse(cId);
            if (p == null)
            {
                throw new KeyNotFoundException($"StudentCourse with courseId {cId} not found.");
            }
            return new BLLStudentCourse
            {
                StudentId = p.StudentId,
                CourseId = p.CourseId,
                RegistrationDate = p.RegistrationDate ?? DateTime.MinValue // Handle nullable DateTime
            };  
        }
        public List<BLLStudentCoursePerfect> GetByIdStudent(int sId)
        {
          List<BLLStudentCoursePerfect> lst = new List<BLLStudentCoursePerfect>();
          var lstFromDal = dal.StudentCourses.GetByIdStudent(sId);
            foreach (var item in lstFromDal)
            {
                BLLStudentCoursePerfect blc = new BLLStudentCoursePerfect
                {
                    StudentId = item.StudentId,
                    CourseId = item.CourseId,
                    CourseName = dal.Courses.GetById(item.CourseId).CouresName,
                    InstructorId = dal.Courses.GetById(item.CourseId).InstructorId,
                    RegistrationDate = item.RegistrationDate ?? DateTime.MinValue // Handle nullable DateTime
                };
                lst.Add(blc);  
            }
            return lst;
        }
        public void Delete(BLLStudentCourse studentCourse)
        {
            var m = dal.StudentCourses.GetById(studentCourse.CourseId, studentCourse.StudentId);

            if (m == null)
            {
                throw new KeyNotFoundException($"StudentCourse with courseId {studentCourse.CourseId} & studentId {studentCourse.StudentId} not found.");
            }

            dal.StudentCourses.Delete(m);
        }


        public void Update(BLLStudentCourse studentCourse)
        {
            var m = dal.StudentCourses.GetById(studentCourse.CourseId, studentCourse.StudentId);
            m.StudentId = studentCourse.StudentId;
            m.CourseId = studentCourse.CourseId;
            m.RegistrationDate = studentCourse.RegistrationDate;

            dal.StudentCourses.Update(m);
        }
    }
}




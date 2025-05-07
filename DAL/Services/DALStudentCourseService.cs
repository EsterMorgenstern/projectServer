using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALStudentCourseService : IDALStudentCourse
    {
        Dbcontext dbcontext;
        public DALStudentCourseService(Dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(StudentCourse studentCourse)
        {
            dbcontext.StudentCourses.Add(studentCourse);
            dbcontext.SaveChanges();
        }

        public void Delete(StudentCourse studentCourse)
        {
            dbcontext.StudentCourses.Remove(studentCourse);
            dbcontext.SaveChanges();
        }

        public List<StudentCourse> Get()
        {
            return  dbcontext.StudentCourses.ToList();
        }

        public StudentCourse GetById(int cId, int sId)
        {
            return dbcontext.StudentCourses.SingleOrDefault(x => x.CourseId == cId && x.StudentId == sId)
                   ?? throw new InvalidOperationException("StudentCourse not found.");
        }
        public StudentCourse GetByIdCourse(int cId)
        {
			var StudentCourse = dbcontext.StudentCourses.SingleOrDefault(x => x.CourseId == cId);
			if (StudentCourse == null)
			{
				throw new KeyNotFoundException($"StudentCourse with ID {cId} not found.");
			}
			return StudentCourse;
		}
        public List<StudentCourse> GetByIdStudent(int sId)
        {
            var lst = dbcontext.StudentCourses.ToList();
            var lst2 = lst.FindAll(x => x.StudentId == sId);
            return lst2;
        }
        public void Update(StudentCourse studentCourse)
        {
            dbcontext.StudentCourses.Update(studentCourse);
            dbcontext.SaveChanges();
        }
    }
}

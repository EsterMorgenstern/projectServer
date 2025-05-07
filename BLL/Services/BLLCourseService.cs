using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLCourseService : IBLLCourse
    {
        IDAL dal;
        public BLLCourseService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLCourse course)
        {
           
        }

        public void Delete(BLLCourse course)
        {
            foreach (var item in dal.StudentCourses.Get())
            {
                if (item.CourseId == course.CourseId)
                {
                  dal.StudentCourses.Delete(dal.StudentCourses.GetByIdCourse(item.CourseId)); 
                }
            }
            dal.Courses.Delete(dal.Courses.GetById(course.CourseId));

        }

        public List<BLLCourse> Get()
        {
            return dal.Courses.Get().Select(c => new BLLCourse()
            {
                CourseId = c.CourseId,
                CouresName = c.CouresName,
                Description = c.Description,
            }).ToList();
        }

        public BLLCourse GetById(int id)
        {
            Course c = dal.Courses.GetById(id);
            BLLCourse blc = new BLLCourse()
            {
                CourseId = c.CourseId,
                    CouresName = c.CouresName,
                Description = c.Description,    
            };
            return blc;
        }

        public void Update(BLLCourse course)
        {
            Course c = dal.Courses.GetById(course.CourseId);
            if (c != null)
            {
                c.CourseId = course.CourseId;
                c.CouresName = course.CouresName;
                c.Description = course.Description;

                dal.Courses.Update(c);
            }
            else
            {
                throw new KeyNotFoundException($"Course with id {course.CourseId} not found.");
            }   
        }
    }
}

using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController
    {
        IBLLStudentCourse studentCourses;

        public StudentCourseController(IBLL manager)
        {
            studentCourses = manager.StudentCourses;
        }

        [HttpGet("GetAll")]
        public List<BLLStudentCourse> Get()
        {
            return studentCourses.Get();
        }

        
        [HttpGet("getById/{id}")]
        public BLLStudentCourse GetById(int id)
        {
            return studentCourses.GetById(id);
        }
        [HttpPost("Add")]
        public void Create(BLLStudentCourse studentCourse)
        {
            studentCourses.Create(studentCourse);
        }
        [HttpPut("Update")]
        public void Update(BLLStudentCourse studentCourse)
        {
            studentCourses.Update(studentCourse);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLStudentCourse studentCourse)
        {
            studentCourses.Delete(studentCourse);
        }
    }
}

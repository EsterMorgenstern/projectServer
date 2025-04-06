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
            studentCourses = (IBLLStudentCourse?)manager.StudentCourses;
        }

        [HttpGet("GetAll")]
        public List<BLLStudentCourse> Get()
        {
            return studentCourses.Get();
        }

        [HttpPost("Add")]
        public void Create(BLLStudentCourse studentCourse)
        {
            studentCourses.Create(studentCourse);
        }
        [HttpGet("getById/{id}")]
        public BLLStudentCourse GetById(int id)
        {
            return studentCourses.GetById(id);
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

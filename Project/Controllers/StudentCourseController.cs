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

        [HttpGet("getById/{cId}/{sId}")]
        public BLLStudentCourse GetById(int cId, int sId)
        {
            return studentCourses.GetById(cId, sId);
        }
        [HttpGet("getByIdStudent/{sId}")]
        public List<BLLStudentCoursePerfect> GetByIdStudent(int sId)
        {
            return studentCourses.GetByIdStudent(sId);
        }

        [HttpPost("Add")]
        public void Create([FromBody] BLLStudentCourse studentCourse)
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

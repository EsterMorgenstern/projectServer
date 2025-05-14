using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController
    {
        IBLLCourse courses;

        public CourseController(IBLL manager)
        {
            courses =manager.Courses;
        }

        [HttpGet("GetAll")]
        public List<BLLCourse> Get()
        {
            return courses.Get();
        }

        
        [HttpGet("getById/{id}")]
        public BLLCourse GetById(int id)
        {
            return courses.GetById(id);
        }
        [HttpPost("Add")]
        public void Create(BLLCourse course)
        {
            courses.Create(course);
        }
        [HttpPut("Update")]
        public void Update(BLLCourse course)
        {
            courses.Update(course);
        }
        [HttpDelete("Delete")]
        public void Delete(int courseId)
        {
            courses.Delete(courseId);
        }
    }
}

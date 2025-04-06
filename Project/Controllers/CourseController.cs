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
            courses = (IBLLCourse?)manager.Courses;
        }

        [HttpGet("GetAll")]
        public List<BLLCourse> Get()
        {
            return courses.Get();
        }

        [HttpPost("Add")]
        public void Create(BLLCourse course)
        {
            courses.Create(course);
        }
        [HttpGet("getById/{id}")]
        public BLLCourse GetById(int id)
        {
            return courses.GetById(id);
        }
        [HttpPut("Update")]
        public void Update(BLLCourse course)
        {
            courses.Update(course);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLCourse course)
        {
            courses.Delete(course);
        }
    }
}

using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;



namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        IBLLStudent students;

        public StudentController(IBLL manager)
        {
            students = manager.Students;
        }

        [HttpGet("GetAll")]
        public List<BLLStudent> Get()
        {
            return students.Get();
        }

        [HttpPost("Add")]
        public void Create(BLLStudent student)
        {
            students.Create(student);
        }
        [HttpGet("getById/{id}")]
        public BLLStudent GetById(int id)
        {
            return students.GetById(id);
        }
        [HttpPut("Update")]
        public void Update(BLLStudent student)
        {
            students.Update(student);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLStudent student)
        {
            students.Delete(student);
        }
    }
}

using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private const string V = "getById/{id}";
        IBLLInstructor instructors;

        public InstructorController(IBLL manager)
        {
            instructors = manager.Instructors;
        }

        [HttpGet("GetAll")]
        public List<BLLInstructor> Get()
        {
            return instructors.Get();
        }
   
        [HttpGet(V)]
        public BLLInstructor GetById(int id)
        {
            return instructors.GetById(id);
        }
        [HttpPost("Add")]
        public void Create(BLLInstructor instructor)
        {
            instructors.Create(instructor);
        }
        [HttpPut("Update")]
        public void Update(BLLInstructor instructor)
        {
            instructors.Update(instructor);
        }

        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            instructors.Delete(id);
        }
    }
}

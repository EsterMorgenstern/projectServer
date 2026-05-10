using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupStudentController : ControllerBase // Fix: Inherit from ControllerBase
    {
        IBLLGroupStudent groupStudents;
        public GroupStudentController(IBLL manager)
        {
            groupStudents = manager.GroupStudents;
        }

        [HttpGet("GetAll")]
        public List<BLLGroupStudent> Get()
        {
            return groupStudents.Get();
        }

        [HttpGet("getById/{id}")]
        public BLLGroupStudent GetById(int id)
        {
            return groupStudents.GetById(id);
        }

        [HttpGet("GetByStatus/{status}")]
        public List<BLLGroupStudentBasic> GetByStatus(string status)
        {
            return groupStudents.GetByStatus(status);
        }

        [HttpGet("getByStudentId/{id}")]
        public List<BLLGroupStudentPerfect> GetByStudentId(int id)
        {
            return groupStudents.GetByStudentId(id);
        }

        [HttpGet("GetByStudentName/{firstName}/{lastName}")]
        public List<BLLGroupStudentPerfect> GetByStudentName(string firstName, string lastName)
        {
            return groupStudents.GetByStudentName(firstName, lastName);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] BLLGroupStudent groupStudent)
        {
            var result = groupStudents.Create(groupStudent);
            return Ok(result);
        }

        [HttpPut("Update")]
        public void Update(BLLGroupStudentSecondly groupStudent)
        {
            groupStudents.Update(groupStudent);
        }

        [HttpDelete("Delete")]
        public void Delete(int gsId)
        {
            groupStudents.Delete(gsId);
        }
    }
}

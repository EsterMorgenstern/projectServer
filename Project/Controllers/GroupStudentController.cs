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
        public IActionResult Create([FromBody] BLLGroupStudent model)
        {
            if (model == null)
            {
                return BadRequest(new
                {
                    success = false,
                    errorCode = "ValidationError",
                    message = "Body חסר או לא תקין"
                });
            }

            var result = groupStudents.Create(model);

            if (result.Success)
            {
                return Ok(result);
            }

            if (result.ErrorCode == "AlreadyExists")
            {
                return Conflict(result); // 409
            }

            if (result.ErrorCode == "ValidationError")
            {
                return BadRequest(result); // 400
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result);
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
        [HttpDelete("DeleteCompletely")]
        public void DeleteCompletely(int gsId)
        {
            groupStudents.DeleteCompletely(gsId);
        }
    }
}

using BLL.Api;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController
    {
        IBLLAttendance attendances;
        public AttendanceController(IBLL manager)
        {
            attendances= manager.Attendances;
        }
        [HttpGet("GetAll")]
        public List<BLLAttendance> Get()
        {
            return attendances.Get();
        }


        [HttpGet("getById/{id}")]
        public BLLAttendance GetById(int id)
        {
            return attendances.GetById(id);
        }
        [HttpPost("Add")]
        public void Create(BLLAttendance attendance)
        {
            attendances.Create(attendance);
        }
        [HttpPut("Update")]
        public void Update(BLLAttendance attendance)
        {
            attendances.Update(attendance);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLAttendance attendance)
        {
            //attendances.Delete(course);
        }
    }
}

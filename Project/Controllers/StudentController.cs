﻿using BLL.Api;
using BLL.Models;
using DAL.Models;
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

        
        [HttpGet("getById/{id}")]
        public BLLStudent GetById(int id)
        {
            return students.GetById(id);
        }
        [HttpPost("Add")]
        public void Create([FromBody] BLLStudent student)
        {
            students.Create(student);
        }
        [HttpPut("Update")]
        public void Update(BLLStudent student)
        {
            students.Update(student);
        }
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            students.Delete(id);
        }
    }
}

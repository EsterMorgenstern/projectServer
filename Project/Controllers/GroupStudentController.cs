﻿using BLL.Api;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupStudentController
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
        [HttpPost("Add")]
        public void Create(BLLGroupStudent groupStudent)
        {
            groupStudents.Create(groupStudent);
        }
        [HttpPut("Update")]
        public void Update(BLLGroupStudent groupStudent)
        {
            groupStudents.Update(groupStudent);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLGroupStudent groupStudent)
        {
           // groupStudents.Delete(groupStudent);
        }
    }
}

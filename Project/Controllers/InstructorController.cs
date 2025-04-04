﻿using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class InstructorController
    {
        IBLLInstructor instructors;

        public InstructorController(IBLL manager)
        {
            instructors = (IBLLInstructor?)manager.Instructors;
        }

        [HttpGet("GetAll")]
        public List<BLLInstructor> Get()
        {
            return instructors.Get();
        }

        [HttpPost("Add")]
        public void Create(BLLInstructor instructor)
        {
            instructors.Create(instructor);
        }
        [HttpGet("getById/{id}")]
        public BLLInstructor GetById(int id)
        {
            return instructors.GetById(id);
        }
        [HttpPut("Update")]
        public void Update(BLLInstructor instructor)
        {
            instructors.Update(instructor);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLInstructor instructor)
        {
            instructors.Delete(instructor);
        }
    }
}

﻿using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController
    {
        IBLLGroup groups;
        public GroupController(IBLL manager)
        {
            groups = manager.Groups;
        }
        [HttpGet("GetAll")]
        public List<BLLGroupDetailsPerfect> Get()
        {
            return groups.Get();
        }


        [HttpGet("getById/{id}")]
        public BLLGroup GetById(int id)
        {
            return groups.GetById(id);
        }
        [HttpGet("getGroupsByCourseId/{id}")]
        public List<BLLGroup> GetGroupsByCourseId(int id)
        {
            return groups.GetGroupsByCourseId(id);
        }
        [HttpGet("GetGroupsByDay/{dayOfWeek}")]
        public List<BLLGroupDetails> GetGroupsByDay(string dayOfWeek)
        {
            return groups.GetGroupsByDayOfWeek(dayOfWeek);
        }

        [HttpGet("getGroupsByInstructorId/{id}")]
        public List<BLLGroupDetailsPerfect> GetGroupsByInstructorId(int id)
        {
            return groups.GetGroupsByInstructorId(id);
        }
        [HttpGet("GetStudentsByGroupId/{id}")]
        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int id)
        {
            return groups.GetStudentsByGroupId(id);
        }
        [HttpGet("FindBestGroupsForStudent/{studentId}")]
        public List<BLLGroupDetailsPerfect> FindBestGroupsForStudent(int studentId, int maxResults = 5)
        {
            return groups.FindBestGroupsForStudent(studentId, maxResults);
        }

        [HttpGet("FindBestGroupForStudent/{studentId}")]
        public ActionResult<BLLGroupDetailsPerfect> FindBestGroupForStudent(int studentId)
        {
            return groups.FindBestGroupForStudent(studentId);


        }

        [HttpPost("Add")]
        public void Create(BLLGroup group)
        {
            groups.Create(group);
        }
        [HttpPut("Update")]
        public void Update(BLLGroup group)
        {
            groups.Update(group);
        }
        [HttpDelete("Delete")]
        public void Delete(int groupId)
        {
            groups.Delete(groupId);
        }
    }
}

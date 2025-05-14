using BLL.Api;
using BLL.Models;
using DAL.Models;
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
        public List<BLLGroup> Get()
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

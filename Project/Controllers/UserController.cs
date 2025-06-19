using BLL.Api;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        IBLLUser users;
        public UserController(IBLL manager)
        {
            users = manager.Users;
        }
        [HttpGet("GetAll")]
        public List<BLLUser> Get()
        {
            return users.Get();
        }


        [HttpGet("getById/{id}")]
        public BLLUser GetById(int id)
        {
            return users.GetById(id);
        }
        [HttpPost("Add")]
        public void Create([FromBody] BLLUser user)
        {
            users.Create(user);
        }
        [HttpPut("Update")]
        public void Update(BLLUser user)
        {
            users.Update(user);
        }
        [HttpDelete("Delete")]
        public void Delete(int userId)
        {
            users.Delete(userId);
        }

    }
}

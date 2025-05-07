using BLL.Api;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace server.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController
    {
        IBLLBranch branches;
        public BranchController(IBLL manager)
        {
            branches = manager.Branches;
        }
        [HttpGet("GetAll")]
        public List<BLLBranch> Get()
        {
            return branches.Get();
        }


        [HttpGet("getById/{id}")]
        public BLLBranch GetById(int id)
        {
            return branches.GetById(id);
        }
        [HttpPost("Add")]
        public void Create(BLLBranch branche)
        {
            branches.Create(branche);
        }
        [HttpPut("Update")]
        public void Update(BLLBranch branche)
        {
            branches.Update(branche);
        }
        [HttpDelete("Delete")]
        public void Delete(BLLBranch branche)
        {
           // branches.Delete(branche);
        }
    }
}

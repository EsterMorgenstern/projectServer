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
        public void Create(BLLBranch branch)
        {
            branches.Create(branch);
        }
        [HttpPut("Update")]
        public void Update(BLLBranch branch)
        {
            branches.Update(branch);
        }
        [HttpDelete("Delete")]
        public void Delete(int branchId)
        {
              branches.Delete(branchId);
        }
    }
}

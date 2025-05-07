using BLL.Api;
using BLL.Models;
using DAL.Api;

namespace BLL.Services
{
    public class BLLBranchService : IBLLBranch
    {
        IDAL dal;
        public BLLBranchService(IDAL dal)
        {
            this.dal = dal;
        }
        public List<BLLBranch> Get()
        {
           return dal.Branches.Get().Select(b => new BLLBranch()
           {
               BranchId = b.BranchId,
               Name = b.Name,
               Address = b.Address,
               MaxGroupSize = b.MaxGroupSize,   
           }).ToList(); 
        }
        public void Create(BLLBranch branch)
        {
            throw new NotImplementedException();
        }
        public BLLBranch GetById(int id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public void Update(BLLBranch branch)
        {
            throw new NotImplementedException();
        }
    }


}

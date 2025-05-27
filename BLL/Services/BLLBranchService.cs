using System.Net;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLBranchService : IBLLBranch
    {
        private readonly IDAL dal;
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
               City = b.City
           }).ToList(); 
        }
        public void Create(BLLBranch branch)
        {
            Branch b = new Branch()
            {
                BranchId = branch.BranchId,
                Name = branch.Name,
                Address = branch.Address,
                MaxGroupSize = branch.MaxGroupSize,
                City= branch.City
            };
            dal.Branches.Create(b);
        }
        public BLLBranch GetById(int id)
        {
            Branch b = dal.Branches.GetById(id);
            BLLBranch blc = new BLLBranch()
            {
                BranchId = b.BranchId,
                Name = b.Name,
                Address = b.Address,
                MaxGroupSize = b.MaxGroupSize,
                City = b.City
            };
            return blc;
        }
       
        public void Delete(int id)
        {
            dal.Branches.Delete(id);
        }
        public void Update(BLLBranch branch)
        {
            Branch b = dal.Branches.GetById(branch.BranchId);
            if (b != null)
            {
                b.BranchId = branch.BranchId;
                b.Name = branch.Name;
                b.Address = branch.Address;
                b.MaxGroupSize = branch.MaxGroupSize;
                b.City = branch.City;   

                dal.Branches.Update(b);
            }
            else
            {
                throw new KeyNotFoundException($"Branch with id {branch.BranchId} not found.");
            }
        }
    }







}

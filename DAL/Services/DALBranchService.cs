using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALBranchService:IDALBranch
    {
        dbcontext dbcontext;
        public DALBranchService(dbcontext data)
        {
            dbcontext = data;
        }
        public void Create(Branch branch)
        {
            dbcontext.Branches.Add(branch);
            dbcontext.SaveChanges();
        }
        public void Delete(int id)
        {
            var trackedBranch = dbcontext.Branches.Find(id);
            if (trackedBranch != null)
            {
                dbcontext.Branches.Remove(trackedBranch);
                dbcontext.SaveChanges();
            }
        }

        public void Delete(Branch branch)
        {
            var trackedBranch = dbcontext.Branches.Find(branch.BranchId);
            if (trackedBranch != null)
            {
                dbcontext.Branches.Remove(trackedBranch);
                dbcontext.SaveChanges();
            }   
        }

        public List<Branch> Get()
        {
            return dbcontext.Branches.ToList();
        }
        public Branch GetById(int id)
        {
            var branch = dbcontext.Branches.SingleOrDefault(x => x.BranchId == id);
            if (branch == null)
            {
                throw new KeyNotFoundException($"Branch with ID {id} not found.");
            }
            return branch;
        }
        public void Update(Branch branch)
        {
            dbcontext.Branches.Update(branch);
            dbcontext.SaveChanges();
        }   
    }
}

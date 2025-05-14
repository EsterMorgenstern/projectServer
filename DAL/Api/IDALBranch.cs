using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALBranch
    {
        List<Branch> Get();
        void Create(Branch branch);
        Branch GetById(int id);
        void Delete(int branchId);
        void Update(Branch branch);
    }
}

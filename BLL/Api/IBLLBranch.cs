using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLBranch
    {
        List<BLLBranchDetails> Get();
        void Create(BLLBranch branch);
        public BLLBranch GetById(int id);
        public void Delete(int id);
        public void Update(BLLBranch branch);   
    }
}

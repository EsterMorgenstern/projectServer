using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLInstructor
    {
        List<BLLInstructor> Get();
        void Create(BLLInstructor student);
        public BLLInstructor GetById(int id);
        public void Delete(BLLInstructor student);
        public void Update(BLLInstructor student);
    }
}

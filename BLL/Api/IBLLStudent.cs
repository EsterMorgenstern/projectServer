using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using DAL.Models;

namespace BLL.Api
{
    public interface IBLLStudent
    {
        List<BLLStudent> Get();
        void Create(BLLStudent student);
        public BLLStudent GetById(int id);
        public void Delete(BLLStudent student);
        public void Update(BLLStudent student);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLAttendance
    {
        List<BLLAttendance> Get();
        void Create(BLLAttendance attendance);
        public BLLAttendance GetById(int id);
        public void Delete(int id);
        public void Update(BLLAttendance attendance);
    }
}

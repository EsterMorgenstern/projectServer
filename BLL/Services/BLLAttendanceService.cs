using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Api;
using BLL.Models;
using DAL.Api;

namespace BLL.Services
{
    public class BLLAttendanceService: IBLLAttendance
    {
        IDAL dal;
        public BLLAttendanceService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLAttendance attendance)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLAttendance> Get()
        {
            throw new NotImplementedException();
        }

        public BLLAttendance GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLAttendance attendance)
        {
            throw new NotImplementedException();
        }
    }
}

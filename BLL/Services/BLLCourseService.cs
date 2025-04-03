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
    public class BLLCourseService : IBLLCourse
    {
        IDAL dal;
        public BLLCourseService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLCourse course)
        {
            throw new NotImplementedException();
        }

        public void Delete(BLLCourse course)
        {
            throw new NotImplementedException();
        }

        public List<BLLCourse> Get()
        {
            throw new NotImplementedException();
        }

        public BLLCourse GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLCourse course)
        {
            throw new NotImplementedException();
        }
    }
}

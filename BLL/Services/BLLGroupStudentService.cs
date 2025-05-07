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
    public class BLLGroupStudentService : IBLLGroupStudent
    {
        IDAL dal;
        public BLLGroupStudentService(IDAL dal)
        {
            this.dal = dal;
        }
        public void Create(BLLGroupStudent groupStudent)
        {
            throw new NotImplementedException();
        }

        

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLGroupStudent> Get()
        {
            throw new NotImplementedException();
        }

        public BLLGroupStudent GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLInstructor> GetInstructorsByGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public List<BLLGroupStudent> GetStudentsByGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLGroupStudent groupStudent)
        {
            throw new NotImplementedException();
        }
               

        List<Models.BLLGroupStudent> IBLLGroupStudent.Get()
        {
            throw new NotImplementedException();
        }

        Models.BLLGroupStudent IBLLGroupStudent.GetById(int id)
        {
            throw new NotImplementedException();
        }

        List<Models.BLLGroupStudent> IBLLGroupStudent.GetStudentsByGroupId(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}

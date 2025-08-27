using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Api
{
    public interface IBLLGroupStudent
    {
        List<BLLGroupStudent> Get();
        void Create(BLLGroupStudent groupStudent);
        public BLLGroupStudent GetById(int id);
        public List<BLLGroupStudentPerfect> GetByStudentId(int id);
        public void Delete(int id);
        public void DeleteByGsId(int id);
        public void Update(BLLGroupStudentSecondly groupStudent);
        public List<BLLGroupStudent> GetStudentsByGroupId(int groupId);
        public List<BLLInstructor> GetInstructorsByGroupId(int groupId);    
    }
}

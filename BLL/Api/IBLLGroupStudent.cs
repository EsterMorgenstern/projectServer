using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using static BLL.Services.BLLGroupStudentService;

namespace BLL.Api
{
    public interface IBLLGroupStudent
    {
        List<BLLGroupStudent> Get();
        CreateGroupStudentResult Create(BLLGroupStudent groupStudent);
        public BLLGroupStudent GetById(int id);
        List<BLLGroupStudentBasic> GetByStatus(string status);
        public List<BLLGroupStudentPerfect> GetByStudentId(int id);
        public void Delete(int id);
        public void DeleteByGsId(int id);
        public void Update(BLLGroupStudentSecondly groupStudent);
        public List<BLLGroupStudent> GetStudentsByGroupId(int groupId);
        public List<BLLGroupStudentPerfect> GetByStudentName(string firstName, string lastName);

        public List<BLLInstructor> GetInstructorsByGroupId(int groupId);    
    }
}

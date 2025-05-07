using BLL.Api;
using BLL.Models;
using DAL.Api;

namespace BLL.Services
{
    public class BLLGroupService : IBLLGroup
    {
        IDAL dal;
        public BLLGroupService(IDAL dal)
        {
            this.dal = dal;
        }
        public void Create(BLLGroup group)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLGroup> Get()
        {
            return dal.Groups.Get().Select(c => new BLLGroup()
            {
                CourseId = c.CourseId,
                AgeRange=c.AgeRange,
                BranchId=c.BranchId,
                City=c.City,
                DayOfWeek= c.DayOfWeek ,
                GroupId= c.GroupId  ,
                GroupName=c.GroupName,
                Hour=c.Hour,
                InstructorId=c.InstructorId,
                MaxStudents=c.MaxStudents,
                Sector=c.Sector

            }).ToList();
        }

        public BLLGroup GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<BLLInstructor> GetInstructorsByGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public List<Models.BLLGroupStudent> GetStudentsByGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public void Update(BLLGroup group)
        {
            throw new NotImplementedException();
        }
    }
}

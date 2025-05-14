using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

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
            Group g = new Group()
            {
                CourseId = group.CourseId,
                BranchId = group.BranchId,
                AgeRange = group.AgeRange,
                DayOfWeek = group.DayOfWeek,
                GroupName = group.GroupName,
                Hour = group.Hour,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                InstructorId = group.InstructorId,
                City = group.City,

            };
            dal.Groups.Create(g);
        }

        public void Delete(int id)
        {
            dal.Groups.Delete(id);
        }

        public List<BLLGroup> Get()
        {
            return dal.Groups.Get().Select(c => new BLLGroup()
            {
                CourseId = c.CourseId,
                AgeRange = c.AgeRange,
                BranchId = c.BranchId,
                City = c.City,
                DayOfWeek = c.DayOfWeek,
                GroupId = c.GroupId,
                GroupName = c.GroupName,
                Hour = c.Hour,
                InstructorId = c.InstructorId,
                MaxStudents = c.MaxStudents,
                Sector = c.Sector

            }).ToList();
        }

        public BLLGroup GetById(int id)
        {
            Group group = dal.Groups.GetById(id);
            BLLGroup blg = new BLLGroup()
            {

                CourseId = group.CourseId,
                BranchId = group.BranchId,
                AgeRange = group.AgeRange,
                DayOfWeek = group.DayOfWeek,
                GroupName = group.GroupName,
                Hour = group.Hour,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                InstructorId = group.InstructorId,
                City = group.City
            };
            return blg;
        }

        public List<BLLGroup> GetGroupsByCourseId(int courseId)
        {
            List<Group> lg = dal.Groups.Get();
            List<BLLGroup> bls = new List<BLLGroup>();
            foreach (var group in lg)
            {
                if (group.CourseId == courseId)
                {
                    BLLGroup bl = new BLLGroup()
                    {
                        CourseId = group.CourseId,
                        BranchId = group.BranchId,
                        AgeRange = group.AgeRange,
                        DayOfWeek = group.DayOfWeek,
                        GroupName = group.GroupName,
                        Hour = group.Hour,
                        MaxStudents = group.MaxStudents,
                        Sector = group.Sector,
                        InstructorId = group.InstructorId,
                        City = group.City
                    };
                    bls.Add(bl);
                }
            }
            return bls;

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

using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALGroupService : IDALGroup
    {
        dbcontext dbcontext;
        public DALGroupService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Group group)
        {
            dbcontext.Groups.Add(group);
            dbcontext.SaveChanges();

        }

        public void Delete(int id)
        {
            var trackedGroup = dbcontext.Groups.Find(id);
            if (trackedGroup != null)
            {
                dbcontext.Groups.Remove(trackedGroup);
                dbcontext.SaveChanges();
            }
        }

        public List<Group> Get()
        {
            return dbcontext.Groups.ToList();
        }

        public Group GetById(int id)
        {
            var group = dbcontext.Groups.SingleOrDefault(x => x.GroupId == id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {id} not found.");
            }
            return group;
        }
        public List<Group> GetGroupsByCourseId(int id)
        {
            return dbcontext.Groups.ToList().FindAll(x => x.CourseId == id);
        }


        public List<Group> GetInstructorsByGroupId(int groupId)
        {
            var group = dbcontext.Groups.SingleOrDefault(x => x.GroupId == groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }
            return dbcontext.Groups.Where(x => x.InstructorId == group.InstructorId).ToList();
        }

        public List<Group> GetStudentsByGroupId(int groupId)
        {
            var group = dbcontext.Groups.SingleOrDefault(x => x.GroupId == groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }
            return dbcontext.Groups.Where(x => x.GroupStudents.Any(gs => gs.GroupId == groupId)).ToList();
        }

        public void Update(Group group)
        {
            var trackedGroup = dbcontext.Groups.Find(group.GroupId);
            if (trackedGroup != null)
            {
                trackedGroup.GroupName = group.GroupName;
                trackedGroup.InstructorId = group.InstructorId;
                ////////////////////////////////////
                dbcontext.SaveChanges();
            }
        }
    }
}

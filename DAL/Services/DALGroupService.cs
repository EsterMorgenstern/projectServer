using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALGroupService : IDALGroup
    {
       private readonly dbcontext dbcontext;
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
            if (dbcontext.Groups == null || !dbcontext.Groups.Any())
            {
                return new List<Group>(); // מחזיר רשימה ריקה במקום לזרוק שגיאה
            }

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
        public List<Group> GetGroupsByDayOfWeek(string dayOfWeek)
        {
            return dbcontext.Groups
                .Where(g => g.DayOfWeek.Equals(dayOfWeek))
                .ToList();
        }


        public List<Group> GetGroupsByInstructorId(int instructorId)
        {
            var groups = dbcontext.Groups.Where(x => x.InstructorId == instructorId);
            if (groups == null)
            {
                throw new KeyNotFoundException($"Group with InstructorId {instructorId} not found.");
            }
            return groups.ToList();
        }

        public List<GroupStudent> GetStudentsByGroupId(int groupId)
        {
           var lst= dbcontext.GroupStudents.Where(x => x.GroupId == groupId).ToList();
            if (lst == null)
            {
                throw new KeyNotFoundException($"Students with GroupId {groupId} not found.");
            }
            return lst;
        }

        public void Update(Group group)
        {
            var trackedGroup = dbcontext.Groups.Find(group.GroupId);
            if (trackedGroup != null)
            {
                dbcontext.Groups.Update(trackedGroup);
                dbcontext.SaveChanges();
            }
        }
    }
}

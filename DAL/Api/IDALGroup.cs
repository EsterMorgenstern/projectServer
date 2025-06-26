using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL.Api
{
    public interface IDALGroup
    {
        List<Group> Get();
        void Create(Group group);
        public Group GetById(int id);
        public List<Group> GetGroupsByCourseId(int id);
        public void Delete(int id);
        public void Update( Group group);
        public List<Group> GetGroupsByDayOfWeek(string dayOfWeek);
        public List<GroupStudent> GetStudentsByGroupId(int groupId);
        public List<Group> GetGroupsByInstructorId(int groupId);
    }
}

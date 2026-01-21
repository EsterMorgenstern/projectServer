using DAL.Models;

namespace DAL.Api
{
    public interface IDALGroup
    {
        List<Group> Get();
        int Create(Group group);
        Group GetById(int id);
        Group GetByIdWithIncludes(int id);
        List<Group> GetGroupsByCourseId(int id);
        void Delete(int id);
        void Update(Group group);
        List<Group> GetGroupsByDayOfWeek(string dayOfWeek);
        List<GroupStudent> GetStudentsByGroupId(int groupId);
        List<Group> GetGroupsByInstructorId(int instructorId);
    }
}

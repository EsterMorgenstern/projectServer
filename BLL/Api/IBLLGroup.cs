using BLL.Models;
using BLL.Services;

namespace BLL.Api
{
    public interface IBLLGroup
    {
        List<BLLGroupDetailsPerfect> Get();
        Task CreateAsync(BLLGroup group);
        BLLGroup GetById(int id);
        void Delete(int id);
        void Update(BLLGroup group);
        List<BLLGroup> GetGroupsByCourseId(int courseId);
        List<BLLGroupDetails> GetGroupsByDayOfWeek(string dayOfWeek);

        List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId);
        List<BLLGroupDetailsPerfect> GetGroupsByInstructorId(int instructorId);
        BLLGroupDetailsPerfect FindBestGroupForStudent(int studentId);
        List<BLLGroupDetailsPerfect> FindBestGroupsForStudent(int studentId, int maxResults = 5);
        List<BLLGroupWithStudentsDto> GetAllGroupsWithStudentsSortedByCourse();
        BLLGroupWithStudentsDto GetGroupWithStudentsById(int groupId);
        List<BLLGroupWithStudentsDto> GetGroupsWithStudentsByBranchId(int branchId);
        Task GenerateLessonsForAllExistingGroups(string createdBy);
        BLLGroupDetailsDto GetGroupDetails(int groupId);
        int GetActiveStudentsCountByGroupId(int groupId);



    }
}

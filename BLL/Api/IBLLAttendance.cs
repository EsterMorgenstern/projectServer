using BLL.Models;

namespace BLL.Api
{
    public interface IBLLAttendance
    {
        List<BLLAttendance> Get();
        void Create(BLLAttendance attendance);
        BLLAttendance GetById(int id);
        List<BLLAttendanceRecord> GetAttendanceByGroupAndDate(int groupId, DateOnly date);
        Dictionary<DateOnly, List<BLLAttendanceRecord>> GetAttendanceByGroupAndDateRange(
             int groupId, DateOnly startDate, DateOnly endDate);
        void DeleteByGroupAndDate(int groupId, DateOnly date);
        void Delete(int attendanceId);
        void Update(BLLAttendance attendance);
        Task<List<BLLAttendance>> GetAttendanceByStudent(int studentId);
        void BatchUpdateAttendances(List<BLLAttendance> attendances);
        List<BLLAttendance> GetStudentAttendanceHistory(int studentId, int? month = null, int? year = null);
        BLLStudentAttendanceSummaryDto GetStudentAttendanceSummary(int studentId, int? month = null, int? year = null);
        void CreateAttendanceForNewStudentInGroup(int studentId, int groupId, DateOnly enrollmentDate);





    }

}

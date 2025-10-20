using DAL.Models;

namespace DAL.Api
{
    public interface IDALAttendance
    {
        List<Attendance> Get();
        void Create(Attendance attendance);
        Attendance GetById(int id);
        List<Attendance> GetAttendanceByGroupAndDate(int groupId, DateOnly date);
        List<Attendance> GetByGroupAndDateRange(int groupId, DateOnly startDate, DateOnly endDate);
        void DeleteByGroupAndDate(int groupId, DateOnly date);
        void Delete(int attendanceId);
        void Update(Attendance attendance);
        Task<List<Attendance>> GetAttendanceByStudent(int studentId);
        List<Attendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate);
        List<Attendance> GetAttendanceByGroup(int groupId);
    }
}

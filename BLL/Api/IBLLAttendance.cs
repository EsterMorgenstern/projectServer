using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Api
{
    public interface IBLLAttendance
    {
        List<BLLAttendance> Get();
        void Create(BLLAttendance attendance);
         BLLAttendance GetById(int id);
         List<BLLAttendance> GetAttendanceByGroupAndDate(int groupId, DateOnly date);
         Dictionary<DateOnly, List<BLLAttendanceRecord>> GetAttendanceByGroupAndDateRange(
              int groupId, DateOnly startDate, DateOnly endDate);
         void DeleteByGroupAndDate(int groupId,DateOnly date);
         void Delete(BLLAttendance attendance);
        void Update(BLLAttendance attendance);
        bool SaveAttendanceForDate(int groupId, DateOnly date, List<BLLAttendanceRecord> attendanceRecords);
        List<BLLAttendance> GetAttendanceByStudent(int studentId);
        List<BLLAttendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate);
        BLLAttendanceStatistics GetAttendanceStatistics(int groupId);
        BLLAttendanceStatistics GetAttendanceStatisticsByDateRange(int groupId, DateOnly startDate, DateOnly endDate);
        bool DeleteAttendanceByGroupAndDate(int groupId, DateOnly date);
    }

}

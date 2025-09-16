﻿using BLL.Models;

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
        bool SaveAttendanceForDate(int groupId, DateOnly date, List<BLLAttendanceRecord> attendanceRecords);
        List<BLLAttendance> GetAttendanceByStudent(int studentId);
        List<BLLAttendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate);
        BLLAttendanceStatistics GetAttendanceStatistics(int groupId);
        BLLAttendanceStatistics GetAttendanceStatisticsByDateRange(int groupId, DateOnly startDate, DateOnly endDate);
        BLLStudentAttendanceSummary GetStudentAttendanceSummary(int studentId, int? month = null, int? year = null);
        List<BLLStudentAttendanceHistory> GetStudentAttendanceHistory(int studentId, int? month = null, int? year = null);
        BLLMonthlyReport GetMonthlyReport(int month, int year, int? groupId = null);
        BLLOverallStatistics GetOverallStatistics(int? month = null, int? year = null);
        bool DeleteAttendanceByGroupAndDate(int groupId, DateOnly date);
        bool IsAttendanceMarkedForGroup(int groupId, DateOnly date);
        bool IsAttendanceMarkedForDay(DateOnly date);
        void AutoMarkDailyAttendance();
        void MarkAttendanceForDate(DateOnly date);



    }

}

using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLAttendanceService : IBLLAttendance
    {
        private readonly IDAL dal;

        public BLLAttendanceService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLAttendance attendance)
        {
            Attendance a = new Attendance()
            {
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent,
                Date = attendance.Date
            };
            dal.Attendances.Create(a);
        }

        public void Delete(BLLAttendance attendance)
        {
            Attendance a = new Attendance()
            {
                AttendanceId = attendance.AttendanceId,
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent,
                Date = attendance.Date
            };
            dal.Attendances.Delete(a);
        }

        public List<BLLAttendance> Get()
        {
            return dal.Attendances.Get().Select(b => new BLLAttendance()
            {
                Date = b.Date,
                StudentId = b.StudentId,
                AttendanceId = b.AttendanceId,
                GroupId = b.GroupId,
                WasPresent = b.WasPresent
            }).ToList();
        }

        public BLLAttendance GetById(int id)
        {
            var attendance = dal.Attendances.GetById(id);
            return new BLLAttendance()
            {
                AttendanceId = attendance.AttendanceId,
                Date = attendance.Date,
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent
            };
        }

        public void Update(BLLAttendance attendance)
        {
            Attendance a = new Attendance()
            {
                AttendanceId = attendance.AttendanceId,
                StudentId = attendance.StudentId,
                GroupId = attendance.GroupId,
                WasPresent = attendance.WasPresent,
                Date = attendance.Date
            };
            dal.Attendances.Update(a);
        }

        public bool SaveAttendanceForDate(int groupId, DateOnly date, List<BLLAttendanceRecord> attendanceRecords)
        {
            try
            {
                dal.Attendances.DeleteByGroupAndDate(groupId, date);

                foreach (var record in attendanceRecords)
                {
                    dal.Attendances.Create(new Attendance
                    {
                        GroupId = groupId,
                        StudentId = record.StudentId,
                        Date = date,
                        WasPresent = record.WasPresent
                    });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<BLLAttendanceRecord> GetAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByGroupAndDate(groupId, date);
            return attendanceRecords.Select(a => new BLLAttendanceRecord
            {
                StudentId = a.StudentId ?? 0,
                WasPresent = a.WasPresent ?? false,
                StudentName = GetStudentName(a.StudentId ?? 0)
            }).ToList();
        }

        public Dictionary<DateOnly, List<BLLAttendanceRecord>> GetAttendanceByGroupAndDateRange(
            int groupId, DateOnly startDate, DateOnly endDate)
        {
            var result = new Dictionary<DateOnly, List<BLLAttendanceRecord>>();
            var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(groupId, startDate, endDate);

            foreach (var group in attendanceRecords.GroupBy(a => a.Date))
            {
                result[group.Key ?? DateOnly.MinValue] = group.Select(a => new BLLAttendanceRecord
                {
                    StudentId = a.StudentId ?? 0,
                    WasPresent = a.WasPresent ?? false,
                    StudentName = GetStudentName(a.StudentId ?? 0)
                }).ToList();
            }

            return result;
        }

        public List<BLLAttendance> GetAttendanceByStudent(int studentId)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByStudent(studentId);
            return attendanceRecords.Select(a => new BLLAttendance
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                GroupId = a.GroupId,
                Date = a.Date,
                WasPresent = a.WasPresent
            }).ToList();
        }

        public List<BLLAttendance> GetAttendanceByStudentAndDateRange(int studentId, DateOnly startDate, DateOnly endDate)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByStudentAndDateRange(studentId, startDate, endDate);
            return attendanceRecords.Select(a => new BLLAttendance
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                GroupId = a.GroupId,
                Date = a.Date,
                WasPresent = a.WasPresent
            }).ToList();
        }

        public BLLAttendanceStatistics GetAttendanceStatistics(int groupId)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByGroup(groupId);
            var groupInfo = dal.Groups.GetById(groupId);
            var groupStudents = dal.Groups.GetStudentsByGroupId(groupId);

            var studentStats = new List<BLLStudentAttendanceStats>();

            foreach (var student in groupStudents)
            {
                var studentAttendance = attendanceRecords.Where(a => a.StudentId == student.StudentId).ToList();
                var attendedLessons = studentAttendance.Count(a => a.WasPresent == true);
                var totalLessons = studentAttendance.Count;

                studentStats.Add(new BLLStudentAttendanceStats
                {
                    StudentId = student.StudentId,
                    StudentName = GetStudentName(student.StudentId),
                    TotalLessons = totalLessons,
                    AttendedLessons = attendedLessons,
                    AttendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0
                });
            }

            return new BLLAttendanceStatistics
            {
                GroupId = groupId,
                GroupName = groupInfo.GroupName,
                TotalStudents = groupStudents.Count,
                TotalLessons = attendanceRecords.GroupBy(a => a.Date).Count(),
                AverageAttendanceRate = studentStats.Any() ? studentStats.Average(s => s.AttendanceRate) : 0,
                StudentStats = studentStats
            };
        }

        public BLLAttendanceStatistics GetAttendanceStatisticsByDateRange(int groupId, DateOnly startDate, DateOnly endDate)
        {
            var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(groupId, startDate, endDate);
            var groupInfo = dal.Groups.GetById(groupId);
            var groupStudents = dal.Groups.GetStudentsByGroupId(groupId);

            var studentStats = new List<BLLStudentAttendanceStats>();

            foreach (var student in groupStudents)
            {
                var studentAttendance = attendanceRecords.Where(a => a.StudentId == student.StudentId).ToList();
                var attendedLessons = studentAttendance.Count(a => a.WasPresent == true);
                var totalLessons = studentAttendance.Count;

                studentStats.Add(new BLLStudentAttendanceStats
                {
                    StudentId = student.StudentId,
                    StudentName = GetStudentName(student.StudentId),
                    TotalLessons = totalLessons,
                    AttendedLessons = attendedLessons,
                    AttendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0
                });
            }

            return new BLLAttendanceStatistics
            {
                GroupId = groupId,
                GroupName = groupInfo.GroupName,
                TotalStudents = groupStudents.Count,
                TotalLessons = attendanceRecords.GroupBy(a => a.Date).Count(),
                AverageAttendanceRate = studentStats.Any() ? studentStats.Average(s => s.AttendanceRate) : 0,
                StudentStats = studentStats
            };
        }

        public bool DeleteAttendanceByGroupAndDate(int groupId, DateOnly date)
        {
            try
            {
                dal.Attendances.DeleteByGroupAndDate(groupId, date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // פונקציה עזר לקבלת שם תלמיד
        private string GetStudentName(int studentId)
        {
            try
            {
                var student = dal.Students.GetById(studentId);
                return $"{student.FirstName} {student.LastName}";
            }
            catch
            {
                return "תלמיד לא נמצא";
            }
        }



        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
        }
    }
}

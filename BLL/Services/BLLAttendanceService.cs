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

        public BLLStudentAttendanceSummary GetStudentAttendanceSummary(int studentId, int? month = null, int? year = null)
        {
            var student = dal.Students.GetById(studentId);
            var attendanceRecords = dal.Attendances.GetAttendanceByStudent(studentId);

            if (month.HasValue && year.HasValue)
            {
                attendanceRecords = attendanceRecords.Where(a =>
                    a.Date.HasValue &&
                    a.Date.Value.Month == month.Value &&
                    a.Date.Value.Year == year.Value).ToList();
            }

            var totalLessons = attendanceRecords.Count;
            var attendedLessons = attendanceRecords.Count(a => a.WasPresent == true);
            var absentLessons = totalLessons - attendedLessons;
            var attendanceRate = totalLessons > 0 ? (double)attendedLessons / totalLessons * 100 : 0;

            return new BLLStudentAttendanceSummary
            {
                StudentId = studentId,
                StudentName = $"{student.FirstName} {student.LastName}",
                TotalLessons = totalLessons,
                AttendedLessons = attendedLessons,
                AbsentLessons = absentLessons,
                AttendanceRate = Math.Round(attendanceRate, 1),
                Month = month ?? DateTime.Now.Month,
                Year = year ?? DateTime.Now.Year
            };
        }

        public List<BLLStudentAttendanceHistory> GetStudentAttendanceHistory(int studentId, int? month = null, int? year = null)
        {
            var attendanceRecords = dal.Attendances.GetAttendanceByStudent(studentId);

            if (month.HasValue && year.HasValue)
            {
                attendanceRecords = attendanceRecords.Where(a =>
                    a.Date.HasValue &&
                    a.Date.Value.Month == month.Value &&
                    a.Date.Value.Year == year.Value).ToList();
            }

            var result = new List<BLLStudentAttendanceHistory>();

            foreach (var record in attendanceRecords)
            {
                try
                {
                    var group = dal.Groups.GetById(record.GroupId ?? 0);
                    var course = dal.Courses.GetById(group.CourseId );
                    var branch = dal.Branches.GetById(group.BranchId );
                    var instructor = dal.Instructors.GetById(group.InstructorId );

                    result.Add(new BLLStudentAttendanceHistory
                    {
                        AttendanceId = record.AttendanceId,
                        StudentId = studentId,
                        StudentName = GetStudentName(studentId),
                        GroupId = record.GroupId ?? 0,
                        GroupName = group.GroupName ?? "",
                        CourseName = course.CouresName ?? "",
                        InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                        BranchName = branch.Name ?? "",
                        Date = record.Date ?? DateOnly.MinValue,
                        LessonTime = group.Hour,
                        IsPresent = record.WasPresent ?? false,
                        
                    });
                }
                catch (Exception ex)
                {
                    // לוג השגיאה אבל המשך עם הרשומות האחרות
                    Console.WriteLine($"Error processing attendance record {record.AttendanceId}: {ex.Message}");
                }
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }

        public BLLMonthlyReport GetMonthlyReport(int month, int year, int? groupId = null)
        {
            var startDate = new DateOnly(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var groups = groupId.HasValue ?
                new List<DAL.Models.Group> { dal.Groups.GetById(groupId.Value) } :
                dal.Groups.Get();

            var groupReports = new List<BLLGroupReport>();
            var totalStudents = 0;
            var totalLessons = 0;
            var totalAttendanceRecords = 0;
            var totalPresentRecords = 0;

            foreach (var group in groups)
            {
                try
                {
                    var attendanceRecords = dal.Attendances.GetByGroupAndDateRange(group.GroupId, startDate, endDate);
                    var groupStudents = dal.Groups.GetStudentsByGroupId(group.GroupId);
                    var course = dal.Courses.GetById(group.CourseId );
                    var branch = dal.Branches.GetById(group.BranchId );

                    var groupTotalLessons = attendanceRecords.GroupBy(a => a.Date).Count();
                    var groupTotalAttendance = attendanceRecords.Count;
                    var groupPresentCount = attendanceRecords.Count(a => a.WasPresent == true);
                    var groupAttendanceRate = groupTotalAttendance > 0 ?
                        (double)groupPresentCount / groupTotalAttendance * 100 : 0;

                    groupReports.Add(new BLLGroupReport
                    {
                        GroupId = group.GroupId,
                        GroupName = group.GroupName ?? "",
                        CourseName = course.CouresName ?? "",
                        BranchName = branch.Name ?? "",
                        TotalStudents = groupStudents.Count,
                        TotalLessons = groupTotalLessons,
                        AverageAttendanceRate = Math.Round(groupAttendanceRate, 1)
                    });

                    totalStudents += groupStudents.Count;
                    totalLessons += groupTotalLessons;
                    totalAttendanceRecords += groupTotalAttendance;
                    totalPresentRecords += groupPresentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing group {group.GroupId}: {ex.Message}");
                }
            }

            var overallAttendanceRate = totalAttendanceRecords > 0 ?
                (double)totalPresentRecords / totalAttendanceRecords * 100 : 0;

            return new BLLMonthlyReport
            {
                Month = month,
                Year = year,
                OverallStatistics = new BLLOverallStatistics
                {
                    TotalStudents = totalStudents,
                    TotalLessons = totalLessons,
                    TotalGroups = groups.Count,
                    OverallAttendanceRate = Math.Round(overallAttendanceRate, 1)
                },
                Groups = groupReports
            };
        }

        public BLLOverallStatistics GetOverallStatistics(int? month = null, int? year = null)
        {
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            var report = GetMonthlyReport(currentMonth, currentYear);
            return report.OverallStatistics;
        }

        public void DeleteByGroupAndDate(int groupId, DateOnly date)
        {
            throw new NotImplementedException();
        }
    }
}

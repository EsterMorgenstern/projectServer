using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class BLLStudentHealthFundService : IBLLStudentHealthFund
    {
        private readonly IDAL dal;
        private readonly dbcontext db;

        public BLLStudentHealthFundService(IDAL dal, dbcontext db)
        {
            this.dal = dal;
            this.db = db;
        }

        public List<BLLStudentHealthFundPerfect> Get()
        {
            var studentHealthFunds = db.StudentHealthFunds
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToList();

            if (!studentHealthFunds.Any())
                return new List<BLLStudentHealthFundPerfect>();

            var studentIds = studentHealthFunds
                .Select(x => x.StudentId)
                .Distinct()
                .ToList();

            var shfIds = studentHealthFunds
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            var students = db.Students
                .AsNoTracking()
                .Where(s => studentIds.Contains(s.Id))
                .ToDictionary(s => s.Id, s => s);

            var groupStudents = db.GroupStudents
                .AsNoTracking()
                .Where(gs => studentIds.Contains(gs.StudentId))
                .ToList();

            var groupIds = groupStudents
                .Select(gs => gs.GroupId)
                .Distinct()
                .ToList();

            var groups = db.Groups
                .AsNoTracking()
                .Where(g => groupIds.Contains(g.GroupId))
                .ToDictionary(g => g.GroupId, g => g.GroupName);

            var attendanceStats = db.Attendances
                .AsNoTracking()
                .Where(a => studentIds.Contains(a.StudentId) && a.WasPresent)
                .GroupBy(a => new { a.StudentId, a.HealthFundReport })
                .Select(g => new
                {
                    g.Key.StudentId,
                    g.Key.HealthFundReport,
                    ReportedCount = g.Count(x => x.StatusReport == 1),
                    PendingCount = g.Count(x => x.StatusReport == 3)
                })
                .ToList()
                .ToDictionary(
                    x => x.StudentId + "_" + x.HealthFundReport,
                    x => (x.ReportedCount, x.PendingCount)
                );

            var commitmentsByShfId = db.HealthFundCommitments
                .AsNoTracking()
                .Where(c => shfIds.Contains(c.StudentHealthFundId) && c.IsActive)
                .ToList()
                .GroupBy(c => c.StudentHealthFundId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var groupNamesByStudentId = groupStudents
                .GroupBy(gs => gs.StudentId)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ",
                        g.Select(gs => groups.ContainsKey(gs.GroupId) ? groups[gs.GroupId] : null)
                         .Where(name => !string.IsNullOrWhiteSpace(name))
                         .Distinct()
                    )
                );

            var startDateByStudentId = groupStudents
                .Where(gs => gs.EnrollmentDate.HasValue)
                .GroupBy(gs => gs.StudentId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(x => x.EnrollmentDate!.Value.ToDateTime(TimeOnly.MinValue))
                );

            var result = new List<BLLStudentHealthFundPerfect>(studentHealthFunds.Count);

            foreach (var shf in studentHealthFunds)
            {
                if (!students.TryGetValue(shf.StudentId, out var student))
                    continue;

                attendanceStats.TryGetValue(shf.StudentId + "_" + shf.HealthFundId, out var stats);
                commitmentsByShfId.TryGetValue(shf.Id, out var studentCommitments);
                groupNamesByStudentId.TryGetValue(shf.StudentId, out var groupName);
                startDateByStudentId.TryGetValue(shf.StudentId, out var startDateGroup);

                studentCommitments ??= new List<HealthFundCommitment>();

                result.Add(new BLLStudentHealthFundPerfect
                {
                    Id = shf.Id,
                    StudentId = shf.StudentId,
                    StudentName = student.FirstName + " " + student.LastName,
                    Age = student.Age,
                    City = student.City,
                    StartDateGroup = startDateGroup == default ? DateTime.MinValue : startDateGroup,
                    HealthFundId = shf.HealthFundId,
                    StartDate = shf.StartDate,
                    GroupName = groupName ?? string.Empty,

                    TreatmentsUsed = stats.PendingCount,
                    ReportedTreatments = stats.ReportedCount,
                    CommitmentTreatments = studentCommitments.Count,
                    RegisteredTreatments = studentCommitments.Count(c => c.UsedTreatments > 0),

                    ReferralFilePath = shf.ReferralFilePath,
                    CommitmentFilePath = shf.CommitmentFilePath,
                    Notes = shf.Notes,
                    StandingOrderDay=shf.StandingOrderDay,

                    Commitments = studentCommitments.Select(c => new BLLHealthFundCommitment
                    {
                        Id = c.Id,
                        StudentHealthFundId = c.StudentHealthFundId,
                        CommitmentNumber = c.CommitmentNumber,
                        CommitmentTreatments = c.CommitmentTreatments,
                        UsedTreatments = c.UsedTreatments,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        FilePath = c.FilePath,
                        Notes = c.Notes,
                        IsActive = c.IsActive
                    }).ToList()
                });
            }

            return result
                .OrderBy(x => x.StudentName)
                .ToList();
        }
        public async Task Create(BLLStudentHealthFund studentHealthFund)
        {
            var existing = await dal.StudentHealthFunds.GetActiveByStudentId(studentHealthFund.StudentId);

            if (existing != null)
            {
                throw new InvalidOperationException("לתלמיד כבר קיימת רשומת גביה פעילה.");
            }

            var shf = new StudentHealthFund
            {
                StudentId = studentHealthFund.StudentId,
                HealthFundId = studentHealthFund.HealthFundId,
                StartDate = studentHealthFund.StartDate == default ? DateTime.Now : studentHealthFund.StartDate,
                ReferralFilePath = studentHealthFund.ReferralFilePath,
                CommitmentFilePath = studentHealthFund.CommitmentFilePath,
                Notes = studentHealthFund.Notes,
                IsActive = true,
                EndDate = null,
                StandingOrderDay=null
            };

            await dal.StudentHealthFunds.Create(shf);
        }

        public BLLStudentHealthFund GetById(int id)
        {
            var shf = dal.StudentHealthFunds.GetById(id);

            return new BLLStudentHealthFund
            {
                Id = shf.Id,
                StudentId = shf.StudentId,
                HealthFundId = shf.HealthFundId,
                StartDate = shf.StartDate,
                ReferralFilePath = shf.ReferralFilePath,
                CommitmentFilePath = shf.CommitmentFilePath,
                Notes = shf.Notes,
                StandingOrderDay = shf.StandingOrderDay
            };
        }

        public void Delete(int id)
        {
            dal.StudentHealthFunds.Delete(id).GetAwaiter().GetResult();
        }

        public void Update(BLLStudentHealthFund studentHealthFund)
        {
            var shf = dal.StudentHealthFunds.GetById(studentHealthFund.Id);

            if (shf == null)
            {
                throw new KeyNotFoundException($"StudentHealthFund with ID {studentHealthFund.Id} not found.");
            }

            shf.StudentId = studentHealthFund.StudentId;
            shf.HealthFundId = studentHealthFund.HealthFundId;
            shf.StartDate = studentHealthFund.StartDate;
            shf.ReferralFilePath = studentHealthFund.ReferralFilePath;
            shf.CommitmentFilePath = studentHealthFund.CommitmentFilePath;
            shf.Notes = studentHealthFund.Notes;
            shf.StandingOrderDay = studentHealthFund.StandingOrderDay;

            dal.StudentHealthFunds.Update(shf).GetAwaiter().GetResult();
        }

        public List<DateTime> GetReportedDates(int studentHealthFundId)
        {
            var shf = dal.StudentHealthFunds.GetById(studentHealthFundId);
            if (shf == null)
                return new List<DateTime>();

            var attendances = dal.Attendances.GetAttendanceByStudent(shf.StudentId)
                .GetAwaiter().GetResult() ?? new List<Attendance>();

            var lessonDates = new List<DateTime>();

            foreach (var attendance in attendances)
            {
                if (!attendance.WasPresent)
                    continue;

                if (attendance.StatusReport != 1)
                    continue;

                if (attendance.HealthFundReport != shf.HealthFundId)
                    continue;

                var lesson = dal.Lessons.GetById(attendance.LessonId);
                if (lesson == null)
                    continue;

                lessonDates.Add(lesson.LessonDate.ToDateTime(TimeOnly.MinValue)); 
            }

            return lessonDates
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        public List<DateTime> GetUnreportedDates(int studentHealthFundId)
        {
            var shf = dal.StudentHealthFunds.GetById(studentHealthFundId);
            if (shf == null)
                return new List<DateTime>();

            var attendances = dal.Attendances.GetAttendanceByStudent(shf.StudentId)
                .GetAwaiter().GetResult() ?? new List<Attendance>();

            var lessonDates = new List<DateTime>();

            foreach (var attendance in attendances)
            {
                if (!attendance.WasPresent)
                    continue;

                if (attendance.StatusReport != 3)
                    continue;

                if (attendance.HealthFundReport != shf.HealthFundId)
                    continue;

                var lesson = dal.Lessons.GetById(attendance.LessonId);
                if (lesson == null)
                    continue;

                lessonDates.Add(lesson.LessonDate.ToDateTime(TimeOnly.MinValue)); // Fix: Convert DateOnly to DateTime
            }

            return lessonDates
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        public void AddReportedDate(int studentHealthFundId, DateTime date)
        {
            ReportUnreportedDate(studentHealthFundId, date).GetAwaiter().GetResult();
        }

        public async Task ReportUnreportedDate(int studentHealthFundId, DateTime date)
        {
            var shf = dal.StudentHealthFunds.GetById(studentHealthFundId);
            if (shf == null)
                throw new InvalidOperationException("רשומת קופת חולים לא נמצאה.");

            var attendances = await dal.Attendances.GetAttendanceByStudent(shf.StudentId);

            var matches = new List<BLLAttendance>();

            foreach (var attendance in attendances)
            {
                if (!attendance.WasPresent)
                    continue;

                if (attendance.StatusReport != 3)
                    continue;

                if (attendance.HealthFundReport != shf.HealthFundId)
                    continue;

                var lesson = dal.Lessons.GetById(attendance.LessonId);
                if (lesson == null)
                    continue;

                if (lesson.LessonDate.ToDateTime(TimeOnly.MinValue) == date.Date)
                {
                    matches.Add(new BLLAttendance
                    {
                        AttendanceId = attendance.AttendanceId,
                        LessonId = attendance.LessonId,
                        StudentId = attendance.StudentId,
                        WasPresent = attendance.WasPresent,
                        StatusReport = attendance.StatusReport,
                        HealthFundReport = attendance.HealthFundReport,
                        DateReport = attendance.DateReport,
                        UpdateDate = attendance.UpdateDate,
                        UpdateBy = attendance.UpdateBy
                    });
                }
            }

            if (!matches.Any())
            {
                throw new InvalidOperationException("לא נמצאו נוכחויות מתאימות לדיווח.");
            }

            foreach (var attendance in matches)
            {
                attendance.StatusReport = 1;
                attendance.DateReport = DateOnly.FromDateTime(DateTime.Now.Date);
                attendance.HealthFundReport = shf.HealthFundId;
                attendance.UpdateDate = DateTime.Now;

                await Task.Run(() => dal.Attendances.Update(new Attendance
                {
                    AttendanceId = attendance.AttendanceId,
                    LessonId = attendance.LessonId,
                    StudentId = attendance.StudentId,
                    WasPresent = attendance.WasPresent,
                    StatusReport = attendance.StatusReport,
                    HealthFundReport = attendance.HealthFundReport,
                    DateReport = attendance.DateReport,
                    UpdateDate = attendance.UpdateDate,
                    UpdateBy = attendance.UpdateBy
                }));
            }
        }

        public void UploadFile(int studentHealthFundId, string filePath, string fileType)
        {
            dal.StudentHealthFunds.SaveFilePath(studentHealthFundId, filePath, fileType);
        }

        public async Task<UnreportedTreatmentsSyncResult> ValidateAndFixUnreportedTreatments()
        {
            return await Task.FromResult(new UnreportedTreatmentsSyncResult
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                TotalStudentsProcessed = 0,
                StudentsUpdated = 0,
                TotalDatesAdded = 0,
                TotalDatesRemoved = 0
            });
        }
    }

    public class UnreportedTreatmentsSyncResult
    {
        public bool IsSuccess { get; set; }
        public int TotalStudentsProcessed { get; set; }
        public int StudentsUpdated { get; set; }
        public int TotalDatesAdded { get; set; }
        public int TotalDatesRemoved { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
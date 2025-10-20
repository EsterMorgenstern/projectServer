using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.Win32.SafeHandles;

namespace BLL.Services
{
    public class BLLStudentHealthFundService : IBLLStudentHealthFund
    {
        private readonly IDAL dal;

        public BLLStudentHealthFundService(IDAL dal)
        {
            this.dal = dal;
        }

        public List<BLLStudentHealthFundPerfect> Get()
        {
            // Await the asynchronous call to ensure the result is available before processing
            var studentHealthFunds = dal.StudentHealthFunds.GetAll().Result;

            return studentHealthFunds.Select(shf =>
            {
                Student student = dal.Students.GetById(shf.StudentId);
                string studentFullName = $"{student.FirstName} {student.LastName}";
                int age = student.Age;
                string? city = student.City;
                var groupStudent = dal.GroupStudents.GetByStudentId(shf.StudentId).FirstOrDefault();
                DateTime? startDateGroup = groupStudent?.EnrollmentDate.HasValue == true
                    ? groupStudent.EnrollmentDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null;

                DateTime startDateGroupValue = startDateGroup ?? DateTime.MinValue;

                var groupStudents = dal.GroupStudents.GetByStudentId(shf.StudentId);
                var groupNames = groupStudents
                    .Select(gs => dal.Groups.GetById(gs.GroupId)?.GroupName)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList();

                return new BLLStudentHealthFundPerfect
                {
                    Id = shf.Id,
                    StudentId = shf.StudentId,
                    StudentName = studentFullName,
                    Age = age,
                    City = city,
                    StartDateGroup = startDateGroupValue,
                    HealthFundId = shf.HealthFundId,
                    StartDate = shf.StartDate,
                    GroupName = string.Join(", ", groupNames),
                    TreatmentsUsed = shf.TreatmentsUsed,
                    CommitmentTreatments = shf.CommitmentTreatments,
                    ReferralFilePath = shf.ReferralFilePath,
                    CommitmentFilePath = shf.CommitmentFilePath,
                    Notes = shf.Notes,
                    ReportedTreatments = shf.ReportedTreatments,
                    RegisteredTreatments=shf.RegisteredTreatments
                };
            }).ToList();
        }
        public async Task Create(BLLStudentHealthFund studentHealthFund)
        {
            var shf = new StudentHealthFund
            {
                Id = studentHealthFund.Id,
                StudentId = studentHealthFund.StudentId,
                HealthFundId = studentHealthFund.HealthFundId,
                StartDate = studentHealthFund.StartDate,
                TreatmentsUsed = 0, // נעדכן בהמשך
                CommitmentTreatments = studentHealthFund.CommitmentTreatments,
                ReferralFilePath = studentHealthFund.ReferralFilePath,
                CommitmentFilePath = studentHealthFund.CommitmentFilePath,
                Notes = studentHealthFund.Notes,
                ReportedTreatments = studentHealthFund.ReportedTreatments,
                RegisteredTreatments=studentHealthFund.RegisteredTreatments
            };

            // שמירת ה-StudentHealthFund החדש
            await dal.StudentHealthFunds.Create(shf);

            var attendanceDates = await dal.Attendances.GetAttendanceByStudent(studentHealthFund.StudentId);

            int unreportedCount = 0;

            // יצירת רשומות בטבלת UnreportedDate עבור כל תאריך נוכחות
            foreach (var attendanceDate in attendanceDates)
            {
                if (attendanceDate.Date.HasValue)
                {
                    var unreportedDate = new UnreportedDate
                    {
                        StudentHealthFundId = shf.Id,
                        DateUnreported = attendanceDate.Date.Value.ToDateTime(TimeOnly.MinValue)
                    };

                    await dal.UnreportedDates.Create(unreportedDate);
                    unreportedCount++;
                }
            }

            // עדכון TreatmentsUsed במספר הרשומות שנוצרו
            shf.TreatmentsUsed = unreportedCount;
            await dal.StudentHealthFunds.Update(shf);
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
                TreatmentsUsed = shf.TreatmentsUsed,
                CommitmentTreatments = shf.CommitmentTreatments,
                ReferralFilePath = shf.ReferralFilePath,
                CommitmentFilePath = shf.CommitmentFilePath,
                Notes = shf.Notes,
                ReportedTreatments=shf.ReportedTreatments,
                RegisteredTreatments=shf.RegisteredTreatments
            };
        }

        public void Delete(int id)
        {
            dal.StudentHealthFunds.Delete(id);
        }

        public void Update(BLLStudentHealthFund studentHealthFund)
        {
            var shf = dal.StudentHealthFunds.GetById(studentHealthFund.Id);
            if (shf != null)
            {
                shf.StudentId = studentHealthFund.StudentId;
                shf.HealthFundId = studentHealthFund.HealthFundId;
                shf.StartDate = studentHealthFund.StartDate;
                shf.TreatmentsUsed = studentHealthFund.TreatmentsUsed;
                shf.CommitmentTreatments = studentHealthFund.CommitmentTreatments;
                shf.ReferralFilePath = studentHealthFund.ReferralFilePath;
                shf.CommitmentFilePath = studentHealthFund.CommitmentFilePath;
                shf.Notes = studentHealthFund.Notes;
                shf.RegisteredTreatments = studentHealthFund.RegisteredTreatments;
                shf.ReportedTreatments = studentHealthFund.ReportedTreatments;
                

                dal.StudentHealthFunds.Update(shf);
            }
            else
            {
                throw new KeyNotFoundException($"StudentHealthFund with ID {studentHealthFund.Id} not found.");
            }
        }

        public void AddReportedDate(int studentHealthFundId, DateTime date)
        {
            var existingReportedDate = dal.ReportedDates.GetByStudentHealthFundId(studentHealthFundId)
                .FirstOrDefault(rd => rd.DateReported == date);
            if (existingReportedDate != null)
            {
                throw new InvalidOperationException("The date is already reported.");
            }

            var unreportedDate = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFundId)
                .FirstOrDefault(ud => ud.DateUnreported == date);
            if (unreportedDate != null)
            {
                dal.UnreportedDates.Delete(unreportedDate.Id).Wait();
            }

            var reportedDate = new ReportedDate
            {
                StudentHealthFundId = studentHealthFundId,
                DateReported = date
            };
            dal.ReportedDates.Create(reportedDate);

            var studentHealthFund = dal.StudentHealthFunds.GetById(studentHealthFundId);
            studentHealthFund.ReportedTreatments++;
            dal.StudentHealthFunds.Update(studentHealthFund);
        }

        public List<DateTime> GetReportedDates(int studentHealthFundId)
        {
            var reportedDates = dal.ReportedDates.GetByStudentHealthFundId(studentHealthFundId);
            return reportedDates?.Select(rd => rd.DateReported).ToList() ?? new List<DateTime>();
        }

        public List<DateTime> GetUnreportedDates(int studentHealthFundId)
        {
            var unreportedDates = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFundId);
            return unreportedDates?.Select(ud => ud.DateUnreported).ToList() ?? new List<DateTime>();
        }

    }
}

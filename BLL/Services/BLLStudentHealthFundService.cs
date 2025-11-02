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
        public async Task ReportUnreportedDate(int studentHealthFundId, DateTime date)
        {
            // שליפת התאריך מרשימת התאריכים שלא דווחו
            var unreportedDate = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFundId)
                .FirstOrDefault(ud => ud.DateUnreported == date);

            if (unreportedDate == null)
            {
                throw new InvalidOperationException("The specified date is not in the unreported dates list.");
            }

            // מחיקת התאריך מרשימת התאריכים שלא דווחו
            await dal.UnreportedDates.Delete(unreportedDate.Id);

            // הוספת התאריך לרשימת התאריכים שדווחו
            var reportedDate = new ReportedDate
            {
                StudentHealthFundId = studentHealthFundId,
                DateReported = date
            };
            await dal.ReportedDates.Create(reportedDate);

            // עדכון מספר הטיפולים שדווחו
            var studentHealthFund = dal.StudentHealthFunds.GetById(studentHealthFundId);
            studentHealthFund.ReportedTreatments++;
            studentHealthFund.TreatmentsUsed--;
            await dal.StudentHealthFunds.Update(studentHealthFund);
        }


        public void UploadFile(int studentHealthFundId, string filePath, string fileType)
        {
            dal.StudentHealthFunds.SaveFilePath(studentHealthFundId, filePath, fileType);
        }



            public async Task SynchronizeUnreportedTreatmentsWithAttendance()
            {
                Console.WriteLine("Starting synchronization of unreported treatments with attendance data");

                try
                {
                    var studentsWithHealthFunds = await dal.StudentHealthFunds.GetAll();

                    if (studentsWithHealthFunds == null || !studentsWithHealthFunds.Any())
                    {
                        Console.WriteLine("No students with health funds found");
                        return;
                    }

                    foreach (var studentHealthFund in studentsWithHealthFunds)
                    {
                        await SynchronizeStudentUnreportedTreatments(studentHealthFund);
                    }

                    Console.WriteLine("Synchronization completed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in SynchronizeUnreportedTreatmentsWithAttendance: {ex.Message}");
                }
            }

            private async Task SynchronizeStudentUnreportedTreatments(StudentHealthFund studentHealthFund)
            {
                try
                {
                    Console.WriteLine($"Processing student {studentHealthFund.StudentId}");

                    // קבלת תאריכים קיימים (בדרך המתאימה למערכת שלך)
                    var allUnreportedDates = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFund.Id);
                    var existingUnreportedDates = allUnreportedDates.ToList();

                    // קבלת נתוני נוכחות
                    var studentAttendance = await dal.Attendances.GetAttendanceByStudent(studentHealthFund.StudentId);
                // Fix for CS0019: Convert DateOnly? to DateTime before comparison
                var validAttendanceDates = studentAttendance
                    .Where(a => a.Date.HasValue &&
                               a.Date.Value.ToDateTime(TimeOnly.MinValue) >= studentHealthFund.StartDate &&
                               a.WasPresent == true)
                    .Select(a => a.Date.Value)
                    .Distinct()
                    .ToList();

                    Console.WriteLine($"Student {studentHealthFund.StudentId} has {validAttendanceDates.Count} valid attendance dates");

                    // מציאת תאריכים חדשים להוספה
                    var existingDates = existingUnreportedDates.Select(u => DateOnly.FromDateTime(u.DateUnreported)).ToHashSet();
                    var newDatesToAdd = validAttendanceDates.Where(date => !existingDates.Contains(date)).ToList();

                    // הוספת תאריכים חדשים
                    foreach (var newDate in newDatesToAdd)
                    {
                        var unreportedDate = new UnreportedDate
                        {
                            StudentHealthFundId = studentHealthFund.Id,
                            DateUnreported = newDate.ToDateTime(TimeOnly.MinValue)
                        };

                        await dal.UnreportedDates.Create(unreportedDate);
                        Console.WriteLine($"Added new unreported date {newDate} for student {studentHealthFund.StudentId}");
                    }

                    // מציאת תאריכים למחיקה
                    var validDateSet = validAttendanceDates.ToHashSet();
                    var datesToRemove = existingUnreportedDates
                        .Where(u => !validDateSet.Contains(DateOnly.FromDateTime(u.DateUnreported)))
                        .ToList();

                    // מחיקת תאריכים לא תקפים
                    foreach (var dateToRemove in datesToRemove)
                    {
                        await dal.UnreportedDates.Delete(dateToRemove.Id);
                        Console.WriteLine($"Removed invalid unreported date {DateOnly.FromDateTime(dateToRemove.DateUnreported)} for student {studentHealthFund.StudentId}");
                    }

                    // עדכון סך הטיפולים
                    var finalUnreportedDates = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFund.Id);
                    var updatedTreatmentsUsed = finalUnreportedDates.Count();

                    if (studentHealthFund.TreatmentsUsed != updatedTreatmentsUsed)
                    {
                        studentHealthFund.TreatmentsUsed = updatedTreatmentsUsed;
                        await dal.StudentHealthFunds.Update(studentHealthFund);
                        Console.WriteLine($"Updated TreatmentsUsed for student {studentHealthFund.StudentId} to {updatedTreatmentsUsed}");
                    }

                    Console.WriteLine($"Synchronization completed for student {studentHealthFund.StudentId}. " +
                                     $"Added: {newDatesToAdd.Count}, Removed: {datesToRemove.Count}, " +
                                     $"Total unreported: {updatedTreatmentsUsed}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing student {studentHealthFund.StudentId}: {ex.Message}");
                }
            }

            public async Task SynchronizeStudentUnreportedTreatments(int studentId)
            {
                try
                {
                    var studentHealthFunds = await dal.StudentHealthFunds.GetAll();
                    var studentHealthFund = studentHealthFunds.FirstOrDefault(s => s.StudentId == studentId);

                    if (studentHealthFund == null)
                    {
                        Console.WriteLine($"No health fund data found for student {studentId}");
                        return;
                    }

                    await SynchronizeStudentUnreportedTreatments(studentHealthFund);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in SynchronizeStudentUnreportedTreatments for student {studentId}: {ex.Message}");
                }
            }

            public async Task<UnreportedTreatmentsSyncResult> ValidateAndFixUnreportedTreatments()
            {
                var result = new UnreportedTreatmentsSyncResult();

                try
                {
                    var studentsWithHealthFunds = await dal.StudentHealthFunds.GetAll();
                    result.TotalStudentsProcessed = studentsWithHealthFunds.Count();

                    foreach (var studentHealthFund in studentsWithHealthFunds)
                    {
                        var beforeSync = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFund.Id);
                        var beforeCount = beforeSync.Count();

                        await SynchronizeStudentUnreportedTreatments(studentHealthFund);

                        var afterSync = dal.UnreportedDates.GetByStudentHealthFundId(studentHealthFund.Id);
                        var afterCount = afterSync.Count();

                        if (afterCount > beforeCount)
                            result.TotalDatesAdded += (afterCount - beforeCount);
                        else if (beforeCount > afterCount)
                            result.TotalDatesRemoved += (beforeCount - afterCount);

                        if (beforeCount != afterCount)
                            result.StudentsUpdated++;
                    }

                    result.IsSuccess = true;
                    Console.WriteLine($"Validation completed: {result.StudentsUpdated} students updated, " +
                                     $"{result.TotalDatesAdded} dates added, {result.TotalDatesRemoved} dates removed");
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorMessage = ex.Message;
                    Console.WriteLine($"Error in ValidateAndFixUnreportedTreatments: {ex.Message}");
                }

                return result;
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

            public override string ToString()
            {
                if (!IsSuccess)
                    return $"Sync failed: {ErrorMessage}";

                return $"Sync completed successfully: {StudentsUpdated}/{TotalStudentsProcessed} students updated, " +
                       $"{TotalDatesAdded} dates added, {TotalDatesRemoved} dates removed";
            }
        }
    }



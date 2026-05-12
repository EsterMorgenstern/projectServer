using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace BLL.Services
{
    public class BLLGroupStudentService : IBLLGroupStudent
    {
        private readonly IDAL dal;
        private readonly BLLAttendanceService attendanceService;
        private readonly BLLStudentService studentService;


        public BLLGroupStudentService(IDAL dal, BLLAttendanceService attendanceService, BLLStudentService studentService)
        {
            this.dal = dal;
            this.attendanceService = attendanceService;
            this.studentService = studentService;
            this.studentService = studentService;
        }

        /// <summary>
        /// יצירת חוג לתלמיד כולל יצירת רשומי נוכחות לפי הקבוצה
        /// </summary>
        /// <param name="groupStudent"></param>


public CreateGroupStudentResult Create(BLLGroupStudent groupStudent)
    {
        if (groupStudent == null)
        {
            return new CreateGroupStudentResult
            {
                Success = false,
                ErrorCode = "ValidationError",
                Message = "נתוני הרשמה חסרים"
            };
        }

        if (groupStudent.StudentId <= 0 || groupStudent.GroupId <= 0)
        {
            return new CreateGroupStudentResult
            {
                Success = false,
                ErrorCode = "ValidationError",
                Message = "StudentId או GroupId לא תקינים"
            };
        }

        // בדיקה מוקדמת (ידידותית למשתמש)
        var existing = dal.GroupStudents
            .Get()
            .FirstOrDefault(x => x.GroupId == groupStudent.GroupId && x.StudentId == groupStudent.StudentId);

        if (existing != null)
        {
            return new CreateGroupStudentResult
            {
                Success = false,
                GroupStudentId = existing.GroupStudentId,
                ErrorCode = "AlreadyExists",
                Message = "התלמיד כבר רשום לקבוצה הזאת"
            };
        }

        var entity = new GroupStudent
        {
            GroupId = groupStudent.GroupId,
            StudentId = groupStudent.StudentId,
            IsActive = (byte?)(groupStudent.IsActive ??
                       (studentService.GetStudentStatus(groupStudent.StudentId) == "פעיל" ? 1 : 0)),
            EnrollmentDate = groupStudent.EnrollmentDate ?? DateOnly.FromDateTime(DateTime.Now),
            TrialDate = groupStudent.TrialDate
        };

        try
        {
            dal.GroupStudents.Create(entity);

            var group = dal.Groups.Get().FirstOrDefault(x => x.GroupId == groupStudent.GroupId);
            if (group != null)
            {
                group.MaxStudents = (group.MaxStudents ?? 0) - 1;
                dal.Groups.Update(group);
            }

            var branch = dal.Branches.Get().FirstOrDefault(x => x.BranchId == group?.BranchId);
            if (branch != null)
            {
                branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) + 1;
                dal.Branches.Update(branch);
            }

            bool trialDateNotFound = false;
            if (entity.IsActive == 1)
            {
                attendanceService.CreateAttendanceForNewStudentInGroup(
                    entity.StudentId,
                    entity.GroupId,
                    (DateOnly)entity.EnrollmentDate
                );
            }
            else if (entity.IsActive == 4 && entity.TrialDate.HasValue && entity.TrialDate.Value != DateOnly.MinValue)
            {
                bool created = attendanceService.CreateAttendanceForTrialLesson(
                    entity.StudentId,
                    entity.GroupId,
                    entity.TrialDate.Value
                );
                trialDateNotFound = !created;
            }

            return new CreateGroupStudentResult
            {
                Success = true,
                GroupStudentId = entity.GroupStudentId,
                TrialDateNotFound = trialDateNotFound,
                Message = "נרשם בהצלחה"
            };
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            // הגנה במקרה race condition (2 בקשות בו זמנית)
            return new CreateGroupStudentResult
            {
                Success = false,
                ErrorCode = "AlreadyExists",
                Message = "התלמיד כבר רשום לקבוצה הזאת"
            };
        }
        catch (Exception)
        {
            return new CreateGroupStudentResult
            {
                Success = false,
                ErrorCode = "ServerError",
                Message = "אירעה שגיאה ביצירת הרשמה לקבוצה"
            };
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
            // 2601,2627 = Unique index / constraint violation
            return sqlEx.Number == 2601 || sqlEx.Number == 2627;
        }
        return false;
    }        /// <summary>
             /// הוצאת תלמיד מחוג-שינוי סטטוס ומחיקת נוכחות עתידית
             /// </summary>
             /// <param name="id"></param>
             /// <exception cref="KeyNotFoundException"></exception>
    public async void Delete(int id)
        {

            var groupStudent = dal.GroupStudents.GetById(id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            groupStudent.IsActive = 2;
            dal.GroupStudents.Update(groupStudent);

            var group = dal.Groups.GetById(groupStudent.GroupId);
            if (group != null)
            {
                group.MaxStudents = (group.MaxStudents ?? 0) + 1;
                dal.Groups.Update(group);
            }
            var branch = dal.Branches.Get().ToList().Find(x => x.BranchId == group?.BranchId);
            if (branch != null)
            {
                branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) - 1;
                dal.Branches.Update(branch);
            }

            // שלוף את כל הנוכחויות העתידיות
            var attendances = (await dal.Attendances.GetAttendanceByStudent(groupStudent.StudentId))
                .Where(a => a.DateReport >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(a => a.DateReport)
                .ToList();


            // שמור את 4 הראשונות ומחק את השאר
            var attendancesToDelete = attendances.Skip(4).ToList();

            foreach (var attendance in attendancesToDelete)
            {
                dal.Attendances.Delete(attendance.AttendanceId);
            }

        }
        /// <summary>
        /// מחיקת תלמיד מחוג - מחיקה מלאה של הנוכחויות 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public async void DeleteCompletely(int id)
        {

            var groupStudent = dal.GroupStudents.GetById(id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            // מחיקת כל הנוכחויות של התלמיד בקורס הזה
            var allAttendances = (await dal.Attendances.GetAttendanceByStudent(groupStudent.StudentId))
                .Where(a => a.LessonId != 0 && dal.Lessons.GetById(a.LessonId)?.GroupId == groupStudent.GroupId)
                .ToList();

            foreach (var attendance in allAttendances)
            {
                dal.Attendances.Delete(attendance.AttendanceId);
            }

            // מחיקת הקשר תלמיד-קורס
            dal.GroupStudents.Delete(id);

            // עדכון group ו-branch
            var group = dal.Groups.GetById(groupStudent.GroupId);
            if (group != null)
            {
                group.MaxStudents = (group.MaxStudents ?? 0) + 1;
                dal.Groups.Update(group);
            }
            var branch = dal.Branches.Get().ToList().Find(x => x.BranchId == group?.BranchId);
            if (branch != null)
            {
                branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) - 1;
                dal.Branches.Update(branch);
            }

        }

        /// <summary>
        /// החזרת כל חוגי התלמידים
        /// </summary>
        /// <returns></returns>
        public List<BLLGroupStudent> Get()
        {
            try
            {
                var groupStudents = dal.GroupStudents.Get();
                if (groupStudents == null || !groupStudents.Any())
                {
                    Console.WriteLine("No group students found.");
                    return new List<BLLGroupStudent>(); // מחזיר מערך ריק
                }

                return groupStudents.Select(gs => new BLLGroupStudent
                {
                    GroupStudentId = gs.GroupStudentId,
                    GroupId = gs.GroupId,
                    StudentId = gs.StudentId,
                    EnrollmentDate = gs.EnrollmentDate,
                    TrialDate = gs.TrialDate,
                    IsActive = gs.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group students: {ex.Message}");
                return new List<BLLGroupStudent>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }
        /// <summary>
        /// החזרת חוג תלמיד לפי ID של GroupStudentId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public BLLGroupStudent GetById(int id)
        {
            var groupStudent = dal.GroupStudents.GetById(id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            return new BLLGroupStudent
            {
                GroupStudentId = groupStudent.GroupStudentId,
                GroupId = groupStudent.GroupId,
                StudentId = groupStudent.StudentId,
                EnrollmentDate = groupStudent.EnrollmentDate,
                TrialDate = groupStudent.TrialDate,
                IsActive = groupStudent.IsActive
            };
        }
        /// <summary>
        /// החזרת חוג תלמיד לפי ID של GroupStudentId (שיטה נוספת עם שימוש ב-Get() ו-SingleOrDefault)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public BLLGroupStudent GetByGsId(int id)
        {
            var groupStudent = dal.GroupStudents.Get().SingleOrDefault(x => x.GroupStudentId == id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            return new BLLGroupStudent
            {
                GroupStudentId = groupStudent.GroupStudentId,
                GroupId = groupStudent.GroupId,
                StudentId = groupStudent.StudentId,
                EnrollmentDate = groupStudent.EnrollmentDate,
                TrialDate = groupStudent.TrialDate,
                IsActive = groupStudent.IsActive
            };
        }
        /// <summary>
        /// החזרת כל חוגי התלמידים לפי ID של תלמיד
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<BLLGroupStudentPerfect> GetByStudentId(int id)
        {
            try
            {
                var groupStudents = dal.GroupStudents.Get().Where(gs => gs.StudentId == id).ToList();
                if (groupStudents == null || !groupStudents.Any())
                {
                    Console.WriteLine($"No group students found for student ID {id}.");
                    return new List<BLLGroupStudentPerfect>(); // מחזיר מערך ריק
                }

                return groupStudents.Select(item =>
                {
                    var d = dal.Groups.GetById(item.GroupId);
                    return new BLLGroupStudentPerfect
                    {
                        GroupStudentId = item.GroupStudentId,
                        GroupId = item.GroupId,
                        StudentId = item.StudentId,
                        StudentName = $"{dal.Students.GetById(item.StudentId).FirstName} {dal.Students.GetById(item.StudentId).LastName}",
                        Student = dal.Students.GetById(item.StudentId),
                        EnrollmentDate = item.EnrollmentDate,
                        TrialDate = item.TrialDate,
                        IsActive = item.IsActive,
                        DayOfWeek = d.DayOfWeek,
                        Hour = d.Hour,
                        GroupName = d.GroupName,
                        BranchName = dal.Branches.GetById(d.BranchId).Name,
                        InstructorName = $"{dal.Instructors.GetById(d.InstructorId).FirstName} {dal.Instructors.GetById(d.InstructorId).LastName}",
                        CourseName = dal.Courses.GetById(d.CourseId).CouresName,
                        AgeRange = d?.AgeRange ?? string.Empty,
                        LessonsCompleted = d?.LessonsCompleted,
                        MaxStudents = d?.MaxStudents,
                        NumOfLessons = d?.NumOfLessons
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group students for student ID {id}: {ex.Message}");
                return new List<BLLGroupStudentPerfect>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }
        /// <summary>
        /// החזרת כל חוגי התלמידים לפי שם פרטי ושם משפחה של תלמיד (שיטה חדשה עם שיפור ביצועים על ידי צמצום מספר הקריאות למסד הנתונים)
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public List<BLLGroupStudentPerfect> GetByStudentName(string firstName, string lastName)
        {
            try
            {
                Console.WriteLine("=== Starting simple database test ===");

                // בדיקה אם המסד זמין בכלל
                try
                {
                    var connectionTest = dal.Students.Get().Count();
                    Console.WriteLine($"Database accessible - {connectionTest} students total");
                }
                catch (Exception dbEx)
                {
                    Console.WriteLine($"Database connection failed: {dbEx.Message}");
                    return new List<BLLGroupStudentPerfect>();
                }

                firstName = firstName?.Trim() ?? "";
                lastName = lastName?.Trim() ?? "";

                // נסה חיפוש קטן יותר - רק students מסוימים
                var relevantStudents = dal.Students.Get()
                    .Where(s => !string.IsNullOrEmpty(s.FirstName) &&
                               !string.IsNullOrEmpty(s.LastName) &&
                               s.FirstName.Contains(firstName) &&
                               s.LastName.Contains(lastName))
                    .Take(10) // מגביל ל-10 רשומות
                    .ToList();

                Console.WriteLine($"Found {relevantStudents.Count} matching students");

                if (!relevantStudents.Any())
                {
                    return new List<BLLGroupStudentPerfect>();
                }

                // עכשיו חפש GroupStudents רק עבור הסטודנטים האלה
                var studentIds = relevantStudents.Select(s => s.Id).ToList();
                var relevantGroupStudents = dal.GroupStudents.Get()
                    .Where(gs => studentIds.Contains(gs.StudentId))
                    .ToList();

                Console.WriteLine($"Found {relevantGroupStudents.Count} group students");

                // מיפוי פשוט
                return relevantGroupStudents.Select(gs =>
                {
                    var student = relevantStudents.First(s => s.Id == gs.StudentId);
                    var group = dal.Groups.GetById(gs.GroupId);

                    return new BLLGroupStudentPerfect
                    {
                        GroupStudentId = gs.GroupStudentId,
                        GroupId = gs.GroupId,
                        StudentId = gs.StudentId,
                        StudentName = $"{student.FirstName} {student.LastName}",
                        Student = student,
                        EnrollmentDate = gs.EnrollmentDate,
                        TrialDate = gs.TrialDate,
                        IsActive = gs.IsActive,
                        DayOfWeek = group?.DayOfWeek ?? string.Empty,
                        Hour = group?.Hour,
                        GroupName = group?.GroupName ?? string.Empty,
                        BranchName = group != null ? dal.Branches.GetById(group.BranchId)?.Name : "",
                        InstructorName = group != null ? $"{dal.Instructors.GetById(group.InstructorId)?.FirstName} {dal.Instructors.GetById(group.InstructorId)?.LastName}" : "",
                        CourseName = group != null ? dal.Courses.GetById(group.CourseId)?.CouresName : "",
                        AgeRange = group?.AgeRange ?? string.Empty,
                        LessonsCompleted = group?.LessonsCompleted,
                        MaxStudents = group?.MaxStudents,
                        NumOfLessons = group?.NumOfLessons

                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<BLLGroupStudentPerfect>();
            }
        }
        /// <summary>
        /// החזרת כל המדריכים של קבוצה לפי ID של קבוצה (שיטה חדשה שמחזירה רשימה של מדריכים במקום מדריך אחד, למקרה שיש יותר ממדריך אחד לקבוצה)
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public List<BLLInstructor> GetInstructorsByGroupId(int groupId)
        {
            var group = dal.Groups.GetById(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {groupId} not found.");
            }

            var instructors = dal.Instructors.Get().Where(i => i.Id == group.InstructorId).ToList();
            return instructors.Select(i => new BLLInstructor
            {
                Id = i.Id,
                FirstName = i.FirstName ??= "",
                LastName = i.LastName ??= "",
                Phone = i.Phone,
                Email = i.Email ??= "",
                City = i.City ??= "",
                Sector = i.Sector ??= ""
            }).ToList();
        }
        /// <summary>
        /// שליפה לפי סטטוס
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<BLLGroupStudentBasic> GetByStatus(string status)
        {
            int? statusInt = status?.ToLower() == "all" ? (int?)null
                : int.TryParse(status, out int s) ? s : (int?)null;

            var groupStudents = dal.GroupStudents.GetByStatus(statusInt);
            if (!groupStudents.Any()) return new List<BLLGroupStudentBasic>();

            var studentIds = groupStudents.Select(gs => gs.StudentId).Distinct().ToList();
            var groupIds = groupStudents.Select(gs => gs.GroupId).Distinct().ToList();

            var studentsMap = dal.Students.Get()
                .Where(s => studentIds.Contains(s.Id))
                .GroupBy(s => s.Id)
                .ToDictionary(g => g.Key, g => g.First());

            var groupsMap = dal.Groups.Get()
                .Where(g => groupIds.Contains(g.GroupId))
                .GroupBy(g => g.GroupId)
                .ToDictionary(g => g.Key, g => g.First());

            return groupStudents.Select(gs =>
            {
                studentsMap.TryGetValue(gs.StudentId, out var student);
                groupsMap.TryGetValue(gs.GroupId, out var group);

                return new BLLGroupStudentBasic
                {
                    GroupStudentId = gs.GroupStudentId,
                    StudentId = gs.StudentId,
                    StudentFirstName = student?.FirstName,
                    StudentLastName = student?.LastName,
                    GroupName = group?.GroupName ?? "",
                    IsActive = gs.IsActive,
                    EnrollmentDate = gs.EnrollmentDate,
                    TrialDate = gs.TrialDate
                };
            }).ToList();
        }
        public List<BLLGroupStudent> GetStudentsByGroupId(int groupId)
        {
            var groupStudents = dal.GroupStudents.Get().Where(gs => gs.GroupId == groupId).ToList();
            return groupStudents.Select(gs => new BLLGroupStudent
            {
                GroupStudentId = gs.GroupStudentId,
                GroupId = gs.GroupId,
                StudentId = gs.StudentId,
                EnrollmentDate = gs.EnrollmentDate,
                TrialDate = gs.TrialDate,
                IsActive = gs.IsActive
            }).ToList();
        }

        /// <summary>
        /// עדכון חוג לתלמיד כולל זיהוי מעבר מ-לא פעיל לפעיל וקריאה ליצירת רשומי נוכחות חדשים, וכן זיהוי שינוי בתאריך ההתחלה ועדכון נוכחויות בהתאם
        /// </summary>
        /// <param name="groupStudent"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Update(BLLGroupStudentSecondly groupStudent)
        {
            var existingGroupStudent = dal.GroupStudents.GetById(groupStudent.GroupStudentId);
            if (existingGroupStudent == null)
            {
                Console.WriteLine($"[Update] GroupStudent with ID {groupStudent.GroupStudentId} not found.");
                throw new KeyNotFoundException($"GroupStudent with ID {groupStudent.GroupStudentId} not found.");
            }

            var oldEnrollmentDate = existingGroupStudent.EnrollmentDate;
            var oldIsActive = existingGroupStudent.IsActive;
            var oldTrialDate = existingGroupStudent.TrialDate; // שמור את הערך הישן

            Console.WriteLine($"[Update] Before update: GroupStudentId={groupStudent.GroupStudentId}, OldTrialDate={oldTrialDate}, NewTrialDate={groupStudent.TrialDate}, OldIsActive={oldIsActive}, NewIsActive={groupStudent.IsActive}");

            existingGroupStudent.GroupId = dal.Groups.Get()
                .Where(x => x.GroupName == groupStudent.GroupName)
                .Select(x => x.GroupId)
                .FirstOrDefault();
            existingGroupStudent.StudentId = groupStudent.StudentId;
            existingGroupStudent.EnrollmentDate = groupStudent.EnrollmentDate;
            existingGroupStudent.TrialDate = groupStudent.TrialDate;
            existingGroupStudent.IsActive = groupStudent.IsActive;

            dal.GroupStudents.Update(existingGroupStudent);

            // זיהוי מעבר מ-לא פעיל לפעיל
            bool becameActive = (oldIsActive == 2 || oldIsActive == 3 || oldIsActive == 4 || oldIsActive == null) && groupStudent.IsActive == 1;
            bool enrollmentDateChanged = oldEnrollmentDate != groupStudent.EnrollmentDate;

            Console.WriteLine($"[Update] becameActive={becameActive}, enrollmentDateChanged={enrollmentDateChanged}");

            if (becameActive)
            {
                Console.WriteLine($"[Update] Creating attendance for new active student. StudentId={groupStudent.StudentId}, GroupId={existingGroupStudent.GroupId}, EnrollmentDate={groupStudent.EnrollmentDate}");
                attendanceService.CreateAttendanceForNewStudentInGroup(
                    groupStudent.StudentId,
                    existingGroupStudent.GroupId,
                    groupStudent.EnrollmentDate ?? DateOnly.FromDateTime(DateTime.Now)
                );
            }
            // יצירת נוכחות לשיעור ניסיון אם נוסף או שונה תאריך ניסיון
            else if (
                groupStudent.IsActive == 4 &&
                groupStudent.TrialDate.HasValue &&
                groupStudent.TrialDate.Value != DateOnly.MinValue &&
                (!oldTrialDate.HasValue || oldTrialDate.Value != groupStudent.TrialDate.Value)
            )
            {
                Console.WriteLine($"[Update] Creating trial lesson attendance. StudentId={groupStudent.StudentId}, GroupId={existingGroupStudent.GroupId}, TrialDate={groupStudent.TrialDate}");
                attendanceService.CreateAttendanceForTrialLesson(
                    groupStudent.StudentId,
                    existingGroupStudent.GroupId,
                    groupStudent.TrialDate.Value
                );
            }
            else
            {
                Console.WriteLine("[Update] No attendance created for trial lesson.");
            }
        }

        public class CreateGroupStudentResult
        {
            public bool Success { get; set; }
            public int? GroupStudentId { get; set; }
            public bool TrialDateNotFound { get; set; }
            public string Message { get; set; } = "";
            public string? ErrorCode { get; set; } // "AlreadyExists", "ValidationError", "ServerError"
        }

    }
}

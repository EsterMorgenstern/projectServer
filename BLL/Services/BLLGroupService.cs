using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLGroupService : IBLLGroup
    {
        private readonly IDAL dal;
        public BLLGroupService(IDAL dal)
        {
            this.dal = dal;
        }
        /// <summary>
        /// הוספת קבוצה חדשה
        /// </summary>
        /// <param name="group"></param>
        public void Create(BLLGroup group)
        {
            Group g = new Group()
            {
                GroupId = group.GroupId,
                CourseId = group.CourseId,
                BranchId = group.BranchId,
                AgeRange = group.AgeRange,
                DayOfWeek = group.DayOfWeek,
                GroupName = group.GroupName,
                Hour = group.Hour,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                InstructorId = group.InstructorId,
                NumOfLessons = group.NumOfLessons,
                LessonsCompleted = group.LessonsCompleted,
                StartDate = group.StartDate

            };
            dal.Groups.Create(g);
        }

        /// <summary>
        /// מחיקת קבוצה
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var groupStudents = dal.GroupStudents.Get().Where(x => x.GroupId == id);
            foreach (var item in groupStudents)
            {
                dal.GroupStudents.Delete(item);
            }
            var lessonCancel = dal.LessonCancellations.Get().Where(x => x.GroupId == id);
            foreach (var item in lessonCancel)
            {
                dal.LessonCancellations.Delete(item.Id);
            }
            var attendances = dal.Attendances.GetAttendanceByGroup(id);
            foreach (var item in attendances)
            {
                dal.Attendances.Delete(item.AttendanceId);
            }

            dal.Groups.Delete(id);
        }

        /// <summary>
        /// החזרת הפרטים המלאים של כל הקבוצות
        /// </summary>
        /// <returns>List<BLLGroupDetailsPerfect></returns>
        public List<BLLGroupDetailsPerfect> Get()
        {
            try
            {
                var groups = dal.Groups.Get();
                if (groups == null || !groups.Any())
                {
                    Console.WriteLine("No groups found.");
                    return new List<BLLGroupDetailsPerfect>(); // מחזיר מערך ריק
                }

                return groups.Select(c => new BLLGroupDetailsPerfect()
                {
                    GroupId = c.GroupId,
                    CourseId = c.CourseId,
                    AgeRange = c.AgeRange,
                    BranchId = c.BranchId,
                    DayOfWeek = c.DayOfWeek,
                    GroupName = c.GroupName,
                    Hour = c.Hour,
                    InstructorId = c.InstructorId,
                    MaxStudents = c.MaxStudents,
                    Sector = c.Sector,
                    NumOfLessons = c.NumOfLessons,
                    LessonsCompleted = c.LessonsCompleted,
                    StartDate = c.StartDate,
                    BranchName = dal.Branches.GetById(c.BranchId).Name,
                    CourseName = dal.Courses.GetById(c.CourseId).CouresName
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching groups: {ex.Message}");
                return new List<BLLGroupDetailsPerfect>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        /// <summary>
        /// החזרת קבוצה מסויימת
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BLLGroup GetById(int id)
        {
            try
            {
                var group = dal.Groups.GetById(id);
                if (group != null)
                {
                    return new BLLGroup()
                    {
                        GroupId = group.GroupId,
                        CourseId = group.CourseId,
                        BranchId = group.BranchId,
                        AgeRange = group.AgeRange,
                        DayOfWeek = group.DayOfWeek,
                        GroupName = group.GroupName,
                        Hour = group.Hour,
                        MaxStudents = group.MaxStudents,
                        Sector = group.Sector,
                        InstructorId = group.InstructorId,
                        NumOfLessons = group.NumOfLessons,
                        LessonsCompleted = group.LessonsCompleted,
                        StartDate = group.StartDate
                    };
                }

                Console.WriteLine($"Group with ID {id} not found.");
                return new BLLGroup()
                {
                    GroupId = 0,
                    CourseId = 0,
                    BranchId = 0,
                    AgeRange = string.Empty,
                    DayOfWeek = string.Empty,
                    GroupName = string.Empty,
                    Hour = null,
                    MaxStudents = null,
                    Sector = string.Empty,
                    InstructorId = 0,
                    NumOfLessons = null,
                    LessonsCompleted = null,
                    StartDate = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group with ID {id}: {ex.Message}");
                return new BLLGroup()
                {
                    GroupId = 0,
                    CourseId = 0,
                    BranchId = 0,
                    AgeRange = string.Empty,
                    DayOfWeek = string.Empty,
                    GroupName = string.Empty,
                    Hour = null,
                    MaxStudents = null,
                    Sector = string.Empty,
                    InstructorId = 0,
                    NumOfLessons = null,
                    LessonsCompleted = null,
                    StartDate = null
                };
            }
        }
        /// <summary>
        /// החזרת קבוצות לפי חוג מסויים
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<BLLGroup> GetGroupsByCourseId(int courseId)
        {
            List<Group> lg = dal.Groups.Get();
            List<BLLGroup> bls = new List<BLLGroup>();
            foreach (var group in lg)
            {
                if (group.CourseId == courseId)
                {
                    BLLGroup bl = new BLLGroup()
                    {
                        GroupId = group.GroupId,
                        CourseId = group.CourseId,
                        BranchId = group.BranchId,
                        AgeRange = group.AgeRange,
                        DayOfWeek = group.DayOfWeek,
                        GroupName = group.GroupName,
                        Hour = group.Hour,
                        MaxStudents = group.MaxStudents,
                        Sector = group.Sector,
                        InstructorId = group.InstructorId,
                        StartDate = group.StartDate,
                        NumOfLessons = group.NumOfLessons,
                        LessonsCompleted = group.LessonsCompleted
                    };
                    bls.Add(bl);
                }
            }
            return bls;

        }

        /// <summary>
        /// החזרת קבוצות שמתקיימות ביום מסוים
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns>List<BLLGroupDetails></returns>
        public List<BLLGroupDetails> GetGroupsByDayOfWeek(string dayOfWeek)
        {
            var groups = dal.Groups.GetGroupsByDayOfWeek(dayOfWeek);
            return groups.Select(g => new BLLGroupDetails
            {
                GroupId = g.GroupId,
                GroupName = g.GroupName,
                DayOfWeek = g.DayOfWeek,
                CourseName = dal.Courses.GetById(g.CourseId).CouresName,
                BranchName = dal.Branches.GetById(g.BranchId).Name,
                Hour = g.Hour,
                AgeRange = g.AgeRange,
                MaxStudents = g.MaxStudents,
                Sector = g.Sector,
                StartDate = g.StartDate,
                NumOfLessons = g.NumOfLessons,
                LessonsCompleted = g.LessonsCompleted
            }).ToList();
        }

        /// <summary>
        /// החזרת פרטי קבוצות לפי קוד מדריך
        /// </summary>
        /// <param name="instructorId"></param>
        /// <returns></returns>
        public List<BLLGroupDetailsPerfect> GetGroupsByInstructorId(int instructorId)
        {

            var groups = dal.Groups.GetGroupsByInstructorId(instructorId);
            List<BLLGroupDetailsPerfect> blc = new List<BLLGroupDetailsPerfect>();
            foreach (var group in groups)
            {
                BLLGroupDetailsPerfect bl = new BLLGroupDetailsPerfect()
                {
                    GroupId = group.GroupId,
                    CourseId = group.CourseId,
                    BranchId = group.BranchId,
                    AgeRange = group.AgeRange,
                    DayOfWeek = group.DayOfWeek,
                    GroupName = group.GroupName,
                    Hour = group.Hour,
                    MaxStudents = group.MaxStudents,
                    Sector = group.Sector,
                    InstructorId = group.InstructorId,
                    StartDate = group.StartDate,
                    NumOfLessons = group.NumOfLessons,
                    LessonsCompleted = group.LessonsCompleted,
                    BranchName = dal.Branches.GetById(group.BranchId).Name,
                    CourseName = dal.Courses.GetById(group.CourseId).CouresName
                };
                blc.Add(bl);
            }
            return blc;
        }

        /// <summary>
        /// החזרת רשימת תלמידים לפי קבוצה
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>List<BLLGroupStudentPerfect></returns>
        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId)
        {
            var lst = dal.Groups.GetStudentsByGroupId(groupId);
            List<BLLGroupStudentPerfect> lstgp = new List<BLLGroupStudentPerfect>();
            foreach (var item in lst)
            {
                var d = dal.Groups.GetById(item.GroupId);

                BLLGroupStudentPerfect gspl = new BLLGroupStudentPerfect()
                {
                    StudentId = item.StudentId,
                    StudentName = dal.Students.GetById(item.StudentId).FirstName + " " + dal.Students.GetById(item.StudentId).LastName,
                    Student = dal.Students.GetById(item.StudentId),
                    EnrollmentDate = item.EnrollmentDate,
                    IsActive = item.IsActive,
                    DayOfWeek = d.DayOfWeek,
                    Hour = d.Hour,
                    GroupName = d.GroupName,
                    BranchName = dal.Branches.GetById(d.BranchId).Name,
                    InstructorName = dal.Instructors.GetById(d.InstructorId).FirstName + " " + dal.Instructors.GetById(d.InstructorId).LastName,
                    CourseName = dal.Courses.GetById(d.CourseId).CouresName
                };
                lstgp.Add(gspl);
            }
            return lstgp;
        }

        public List<BLLGroupDetailsPerfect> FindBestGroupsForStudent(int studentId, int maxResults = 5)
        {
            var student = dal.Students.GetById(studentId);
            if (student == null)
            {
                return new List<BLLGroupDetailsPerfect>();
            }

            var groups = dal.Groups.Get();

            var eligibleGroups = groups
                .Where(g =>
                    IsStudentInAgeRange(student.Age, g.AgeRange) &&
                    (string.IsNullOrEmpty(g.Sector) || g.Sector == student.Sector) &&
                    (g.MaxStudents == null || g.MaxStudents > 0)
                )
                .Select(g => new
                {
                    Group = g,
                    MatchScore = CalculateMatchScore(g, student),
                    Priority = CalculatePriority(g, student)
                })
                .OrderByDescending(x => x.MatchScore)  // ציון התאמה גבוה ראשון
                .ThenBy(x => x.Priority)              // עדיפות נמוכה = עדיפות גבוהה
                .ThenByDescending(x => x.Group.MaxStudents) // יותר מקומות פנויים
                .ThenBy(x => x.Group.StartDate)       // מתחיל מוקדם יותר
                .Take(maxResults)
                .ToList();

            var result = new List<BLLGroupDetailsPerfect>();

            foreach (var item in eligibleGroups)
            {
                var group = item.Group;
                var branch = dal.Branches.GetById(group.BranchId);
                var course = dal.Courses.GetById(group.CourseId);
                var instructor =
                    dal.Instructors.GetById(group.InstructorId);

                var bllGroup = new BLLGroupDetailsPerfect()
                {
                    GroupId = group.GroupId,
                    CourseId = group.CourseId,
                    BranchId = group.BranchId,
                    AgeRange = group.AgeRange,
                    DayOfWeek = group.DayOfWeek ?? string.Empty,
                    GroupName = group.GroupName ?? string.Empty,
                    Hour = group.Hour,
                    MaxStudents = group.MaxStudents,
                    Sector = group.Sector,
                    InstructorId = group.InstructorId,
                    StartDate = group.StartDate,
                    NumOfLessons = group.NumOfLessons,
                    LessonsCompleted = group.LessonsCompleted,
                    BranchName = branch?.Name ?? string.Empty,
                    CourseName = course?.CouresName ?? string.Empty,
                    InstructorName = instructor?.FirstName + " " + instructor?.LastName ?? string.Empty,
                    BranchCity = branch?.City ?? string.Empty,
                    BranchAddress = branch?.Address ?? string.Empty,

                    // נתונים נוספים לממשק
                    MatchScore = item.MatchScore,
                    MatchReasons = GenerateMatchReasons(group, student, branch, course)
                };

                result.Add(bllGroup);
            }

            return result;
        }

        // פונקציה לחישוב ציון התאמה (0-100)
        private int CalculateMatchScore(Group group, Student student)
        {
            int score = 50; // ציון בסיס

            // בונוס למקומות פנויים
            if (group.MaxStudents.HasValue && group.MaxStudents > 0)
            {
                score += Math.Min(group.MaxStudents.Value * 2, 20); // עד 20 נקודות
            }

            // בונוס להתאמת מגזר מדויקת
            if (!string.IsNullOrEmpty(group.Sector) && group.Sector == student.Sector)
            {
                score += 15;
            }

            // בונוס לקבוצות שמתחילות בקרוב
            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart >= 0 && daysUntilStart <= 30)
                {
                    score += 10;
                }
            }

            // בונוס לקבוצות עם פחות שיעורים שהושלמו (יותר תוכן נותר)
            if (group.NumOfLessons.HasValue && group.LessonsCompleted.HasValue)
            {
                var remainingLessons = group.NumOfLessons.Value - group.LessonsCompleted.Value;
                if (remainingLessons > group.NumOfLessons.Value * 0.7) // נותרו יותר מ-70% מהשיעורים
                {
                    score += 5;
                }
            }

            return Math.Min(score, 100);
        }

        // פונקציה לחישוב עדיפות (מספר נמוך = עדיפות גבוהה)
        private int CalculatePriority(Group group, Student student)
        {
            int priority = 0;

            // עדיפות גבוהה לקבוצות עם התאמת מגזר מדויקת
            if (string.IsNullOrEmpty(group.Sector) || group.Sector != student.Sector)
            {
                priority += 10;
            }

            // עדיפות נמוכה לקבוצות מלאות
            if (!group.MaxStudents.HasValue || group.MaxStudents <= 0)
            {
                priority += 50;
            }

            // עדיפות לקבוצות שמתחילות בזמן הקרוב
            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart > 60)
                {
                    priority += 20;
                }
            }

            return priority;
        }

        // פונקציה ליצירת סיבות להתאמה
        private List<string> GenerateMatchReasons(Group group, Student student, Branch branch, Course course)
        {
            var reasons = new List<string>();

            if (group.MaxStudents.HasValue && group.MaxStudents > 0)
            {
                reasons.Add($"{group.MaxStudents} מקומות פנויים");
            }

            if (!string.IsNullOrEmpty(group.Sector) && group.Sector == student.Sector)
            {
                reasons.Add($"התאמת מגזר - {group.Sector}");
            }

            if (branch != null && !string.IsNullOrEmpty(branch.City))
            {
                reasons.Add($"סניף ב{branch.City}");
            }

            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart >= 0 && daysUntilStart <= 30)
                {
                    reasons.Add("מתחיל בקרוב");
                }
            }

            if (!string.IsNullOrEmpty(group.AgeRange))
            {
                reasons.Add($"מתאים לגיל {group.AgeRange}");
            }

            if (course != null && !string.IsNullOrEmpty(course.CouresName))
            {
                reasons.Add($"חוג {course.CouresName}");
            }

            return reasons.Count > 0 ? reasons : new List<string> { "קבוצה זמינה" };
        }

        // שמירה על הפונקציה המקורית לתאימות לאחור
        public BLLGroupDetailsPerfect FindBestGroupForStudent(int studentId)
        {
            var bestGroups = FindBestGroupsForStudent(studentId, 1);
            return bestGroups.FirstOrDefault() ?? new BLLGroupDetailsPerfect();
        }



        private bool IsStudentInAgeRange(int age, string? ageRange)
        {
            if (string.IsNullOrEmpty(ageRange))
                return true;

            // פיצול טווח הגילאים למינימום ומקסימום
            var parts = ageRange.Split('-');
            if (parts.Length != 2)
                return false;

            // ניסיון להמיר את החלקים למספרים
            if (int.TryParse(parts[0], out int minAge) && int.TryParse(parts[1], out int maxAge))
            {
                // בדיקה אם הגיל נמצא בטווח
                return age >= minAge && age <= maxAge;
            }

            return false;
        }

        public void Update(BLLGroup group)
        {
            Group existingGroup = dal.Groups.GetById(group.GroupId);
            if (existingGroup == null)
            {
                throw new KeyNotFoundException($"Group with ID {group.GroupId} not found.");
            }
            existingGroup.GroupId = group.GroupId;
            existingGroup.CourseId = group.CourseId;
            existingGroup.BranchId = group.BranchId;
            existingGroup.AgeRange = group.AgeRange;
            existingGroup.DayOfWeek = group.DayOfWeek;
            existingGroup.GroupName = group.GroupName;
            existingGroup.Hour = group.Hour;
            existingGroup.MaxStudents = group.MaxStudents;
            existingGroup.Sector = group.Sector;
            existingGroup.InstructorId = group.InstructorId;
            existingGroup.StartDate = group.StartDate;
            existingGroup.NumOfLessons = group.NumOfLessons;
            existingGroup.LessonsCompleted = group.LessonsCompleted;

            dal.Groups.Update(existingGroup);
        }




    }
}

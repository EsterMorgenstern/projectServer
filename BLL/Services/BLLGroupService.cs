using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services

{
    public class BLLGroupService : IBLLGroup
    {
        private readonly IDAL dal;
        private readonly IBLLLesson lessonService;
        public BLLGroupService(IDAL dal, IBLLLesson lessonService)
        {
            this.dal = dal;
            this.lessonService = lessonService;
        }
        /// <summary>
        /// הוספת קבוצה חדשה כולל הוספת שיעורים
        /// </summary>
        /// <param name="group"></param>
        public async Task CreateAsync(BLLGroup group)
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
           int groupId = dal.Groups.Create(g);

            if (group.StartDate.HasValue) 
            {
                if (group.Hour.HasValue) 
                {
                    await lessonService.GenerateLessonsForGroup(
                        groupId: groupId,
                        startDate: group.StartDate.Value,
                        numOfLessons: group.NumOfLessons ?? 0,
                        dayOfWeek: group.DayOfWeek,
                        lessonHour: group.Hour.Value, 
                        instructorId: group.InstructorId,
                        createdBy: "system"
                    );
                }
                else
                {
                    throw new ArgumentException("Hour cannot be null when creating lessons for a group.");
                }
            }
            else
            {
                throw new ArgumentException("StartDate cannot be null when creating lessons for a group.");
            }
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
            var lessons = dal.Lessons.Get().Where(x => x.GroupId == id);
            foreach (var item in lessons)
            {
                dal.Lessons.Delete(item.LessonId);
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

                return groups.Select(c =>
                {
                    var instructor = dal.Instructors.GetById(c.InstructorId);
                    return new BLLGroupDetailsPerfect()
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
                        InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
                        BranchName = dal.Branches.GetById(c.BranchId).Name,
                        CourseName = dal.Courses.GetById(c.CourseId).CouresName
                    };
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

        /// <summary>
        ///   מתאים לייצוא לאקסל החזרת רשימת תלמידים לפי קבוצה
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>List<BLLGroupWithStudentsDto></returns>
        public BLLGroupWithStudentsDto GetGroupWithStudentsById(int groupId)
        {
            // Fetch the group by ID
            var group = dal.Groups.GetById(groupId);
            if (group == null)
                return null;

            // Pre-fetch all required data in bulk to minimize database calls
            var allStudents = dal.Students.Get(); // Fetch all students once
            var allCourses = dal.Courses.Get(); // Fetch all courses once
            var allBranches = dal.Branches.Get(); // Fetch all branches once
            var allInstructors = dal.Instructors.Get(); // Fetch all instructors once
            var allGroupStudents = dal.GroupStudents.Get(); // Fetch all group-student mappings once

            // Filter students for the current group
            var students = allGroupStudents
                .Where(gs => gs.GroupId == group.GroupId)
                .Select(s =>
                {
                    var student = allStudents.FirstOrDefault(st => st.Id == s.StudentId);
                    return new StudentDto
                    {
                        StudentId = s.StudentId,
                        StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
                        Phone = student?.Phone,
                        City = student?.City,
                        HealthFund = student?.HealthFundForStudent != null
         ? $"{student.HealthFundForStudent.Name} ({student.HealthFundForStudent.FundType})"
         : string.Empty
                    };

                })
                .ToList();

            // Fetch related data for the group
            var course = allCourses.FirstOrDefault(c => c.CourseId == group.CourseId);
            var branch = allBranches.FirstOrDefault(b => b.BranchId == group.BranchId);
            var instructor = allInstructors.FirstOrDefault(i => i.Id == group.InstructorId);

            // Build the result
            return new BLLGroupWithStudentsDto
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                CourseName = course?.CouresName,
                BranchName = branch?.Name,
                AgeRange = group.AgeRange,
                LessonsCompleted = group.LessonsCompleted,
                MaxStudents = group.MaxStudents,
                NumOfLessons = group.NumOfLessons,
                Sector = group.Sector,
                StartDate = group.StartDate,
                Schedule = $"{group.DayOfWeek} {group.Hour?.ToString("HH:mm")}",
                InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
                Students = students
            };
        }


        #region FindBestGroupsForStudent
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
        #endregion


        /// <summary>
        /// עדכון פרטי קבוצה
        /// </summary>
        /// <param name="group"></param>
        /// <exception cref="KeyNotFoundException"></exception>
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


        /// <summary>
        /// החזרת כל הקבוצות עם התלמידים שלהם, ממוינות לפי שם החוג מתאים לייצוא לאקסל
        /// </summary>
        /// <returns>List<BLLGroupWithStudentsDto></returns>
        public List<BLLGroupWithStudentsDto> GetAllGroupsWithStudentsSortedByCourse()
        {
            var groups = dal.Groups.Get();
            if (groups == null || !groups.Any())
                return new List<BLLGroupWithStudentsDto>();

            // Pre-fetch all required data in bulk to minimize database calls
            var allStudents = dal.Students.Get(); // Fetch all students once
            var allCourses = dal.Courses.Get(); // Fetch all courses once
            var allBranches = dal.Branches.Get(); // Fetch all branches once
            var allInstructors = dal.Instructors.Get(); // Fetch all instructors once
            var allGroupStudents = dal.GroupStudents.Get(); // Fetch all group-student mappings once

            var result = groups
                .Select(g =>
                {
                    // Filter students for the current group
                    var students = allGroupStudents
                        .Where(gs => gs.GroupId == g.GroupId)
                        .Select(s =>
                        {
                            var student = allStudents.FirstOrDefault(st => st.Id == s.StudentId);
                            return new StudentDto
                            {
                                StudentId = s.StudentId,
                                StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
                                Phone = student?.Phone,
                                City = student?.City,
                                HealthFund = student?.HealthFundForStudent != null
         ? $"{student.HealthFundForStudent.Name} ({student.HealthFundForStudent.FundType})"
         : string.Empty
                            };
                        })
                        .ToList();

                    // Fetch related data for the group
                    var course = allCourses.FirstOrDefault(c => c.CourseId == g.CourseId);
                    var branch = allBranches.FirstOrDefault(b => b.BranchId == g.BranchId);
                    var instructor = allInstructors.FirstOrDefault(i => i.Id == g.InstructorId);

                    return new BLLGroupWithStudentsDto
                    {
                        GroupId = g.GroupId,
                        GroupName = g.GroupName,
                        CourseName = course?.CouresName,
                        BranchName = branch?.Name,
                        AgeRange = g.AgeRange,
                        LessonsCompleted = g.LessonsCompleted,
                        MaxStudents = g.MaxStudents,
                        NumOfLessons = g.NumOfLessons,
                        Sector = g.Sector,
                        StartDate = g.StartDate,
                        Schedule = $"{g.DayOfWeek} {g.Hour?.ToString("HH:mm")}",
                        InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
                        Students = students
                    };
                })
                .OrderBy(g => g.CourseName)
                .ToList();

            return result;
        }

        /// <summary>
        ///מתאים לייצוא לאקסל החזרת רשימת קבוצות ותלמידים לפי סניף
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>List<BLLGroupWithStudentsDto></returns>
        public List<BLLGroupWithStudentsDto> GetGroupsWithStudentsByBranchId(int branchId)
        {
            // Fetch the branch by ID
            var branch = dal.Branches.GetById(branchId);
            if (branch == null)
                return new List<BLLGroupWithStudentsDto>(); // מחזיר רשימה ריקה אם הסניף לא נמצא

            // Pre-fetch all required data in bulk to minimize database calls
            var allGroups = dal.Groups.Get().Where(g => g.BranchId == branchId).ToList(); // קבוצות בסניף
            var allStudents = dal.Students.Get(); // כל התלמידים
            var allCourses = dal.Courses.Get(); // כל הקורסים
            var allInstructors = dal.Instructors.Get(); // כל המדריכים
            var allGroupStudents = dal.GroupStudents.Get(); // כל החיבורים בין קבוצות לתלמידים

            // Build the result for each group in the branch
            var result = allGroups.Select(group =>
            {
                // Filter students for the current group
                var students = allGroupStudents
                    .Where(gs => gs.GroupId == group.GroupId)
                    .Select(s =>
                    {
                        var student = allStudents.FirstOrDefault(st => st.Id == s.StudentId);
                        return new StudentDto
                        {
                            StudentId = s.StudentId,
                            StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
                            Phone = student?.Phone,
                            City = student?.City,
                            HealthFund = student?.HealthFundForStudent != null ? $"{student.HealthFundForStudent.Name} ({student.HealthFundForStudent.FundType})" : string.Empty
                        };
                    })
                    .ToList();

                // Fetch related data for the group
                var course = allCourses.FirstOrDefault(c => c.CourseId == group.CourseId);
                var instructor = allInstructors.FirstOrDefault(i => i.Id == group.InstructorId);

                // Build the group DTO
                return new BLLGroupWithStudentsDto
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    CourseName = course?.CouresName,
                    BranchName = branch.Name,
                    AgeRange = group.AgeRange,
                    LessonsCompleted = group.LessonsCompleted,
                    MaxStudents = group.MaxStudents,
                    NumOfLessons = group.NumOfLessons,
                    Sector = group.Sector,
                    StartDate = group.StartDate,
                    Schedule = $"{group.DayOfWeek} {group.Hour?.ToString("HH:mm")}",
                    InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
                    Students = students
                };
            }).ToList();

            return result;
        }
        /// <summary>
        /// פונקציה ליצירת שיעורים לכל הקבוצות
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task GenerateLessonsForAllExistingGroups(string createdBy)
        {
            try
            {
                var groups = dal.Groups.Get(); // קבל את כל הקבוצות

                foreach (var group in groups)
                {
                    // דלג על קבוצות שאין להם תאריך התחלה תקין
                    if (group.StartDate == null || group.StartDate == DateOnly.MinValue)
                        continue;

                    // בדוק אם לקבוצה כבר יש שיעורים
                    var existingLessons = dal.Lessons.Get()?.Where(l => l.GroupId == group.GroupId).ToList();
                    if (existingLessons != null && existingLessons.Any())
                        continue; // דלג, כבר יש שיעורים

                    // יצור שיעורים
                    
                    await lessonService.GenerateLessonsForGroup(
                        groupId: group.GroupId,
                    startDate: !group.StartDate.HasValue ? throw new ArgumentException("StartDate cannot be null when generating lessons.") : group.StartDate.Value,
                        numOfLessons: group.NumOfLessons ?? 0,
                        dayOfWeek: group.DayOfWeek,
                        lessonHour: group.Hour ?? throw new ArgumentException("Hour cannot be null when generating lessons."),
                        instructorId: group.InstructorId,
                        createdBy: createdBy
                    );

                    Console.WriteLine($"✅ יוצרו שיעורים לקבוצה: {group.GroupName}");
                }

                Console.WriteLine("✅ סיום יצירת שיעורים לכל הקבוצות");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ שגיאה: {ex.Message}");
                throw;
            }
        }
     /// <summary>
     /// פונקציה שמחזירה פרטים מלאים של קבוצה
     /// </summary>
     /// <param name="groupId"></param>
     /// <returns></returns>
        public BLLGroupDetailsDto GetGroupDetails(int groupId)
        {
            var group = dal.Groups.GetByIdWithIncludes(groupId);
            if (group == null)
                return null;

            var students = group.GroupStudents.Select(gs => gs.Student).ToList();
            var lessons = group.Lessons?.ToList() ?? new List<Lesson>();
            var instructor = group.Instructor;

            return new BLLGroupDetailsDto
            {
                GroupId = group.GroupId,
                BranchId = group.BranchId,
                CourseId = group.CourseId,
                InstructorId = group.InstructorId,
                GroupName = group.GroupName,
                DayOfWeek = group.DayOfWeek,
                Hour = group.Hour,
                AgeRange = group.AgeRange,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                StartDate = group.StartDate,
                NumOfLessons = group.NumOfLessons,
                LessonsCompleted = group.LessonsCompleted,
                Branch = group.Branch,
                Course = group.Course,
                Instructor = instructor,
                Students = students,
                Lessons = lessons
            };
        }


    }
}

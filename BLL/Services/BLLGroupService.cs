using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;
using Group = DAL.Models.Group;

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

        #region Mapper Functions

        /// <summary>
        /// Converts DAL Group to BLLGroup.
        /// </summary>
        private BLLGroup ToBLLGroup(Group dalGroup)
        {
            if (dalGroup == null) return null;
            return new BLLGroup
            {
                GroupId = dalGroup.GroupId,
                CourseId = dalGroup.CourseId,
                BranchId = dalGroup.BranchId,
                AgeRange = dalGroup.AgeRange,
                DayOfWeek = dalGroup.DayOfWeek,
                GroupName = dalGroup.GroupName,
                Hour = dalGroup.Hour,
                MaxStudents = dalGroup.MaxStudents,
                Sector = dalGroup.Sector,
                InstructorId = dalGroup.InstructorId,
                NumOfLessons = dalGroup.NumOfLessons,
                LessonsCompleted = dalGroup.LessonsCompleted,
                StartDate = dalGroup.StartDate,
                IsActive = dalGroup.IsActive,
                Notes = dalGroup.Notes,
                KolKasherGroupNumber = dalGroup.KolKasherGroupNumber
            };
        }

        /// <summary>
        /// Converts BLLGroup to DAL Group.
        /// </summary>
        private Group ToDALGroup(BLLGroup bllGroup)
        {
            if (bllGroup == null) return null;
            return new Group
            {
                GroupId = bllGroup.GroupId,
                CourseId = bllGroup.CourseId,
                BranchId = bllGroup.BranchId,
                AgeRange = bllGroup.AgeRange,
                DayOfWeek = bllGroup.DayOfWeek ?? "",
                GroupName = GenerateGroupName(bllGroup),
                Hour = bllGroup.Hour,
                MaxStudents = bllGroup.MaxStudents,
                Sector = bllGroup.Sector,
                InstructorId = bllGroup.InstructorId,
                NumOfLessons = bllGroup.NumOfLessons,
                LessonsCompleted = bllGroup.LessonsCompleted,
                StartDate = bllGroup.StartDate,
                IsActive = bllGroup.IsActive,
                Notes = bllGroup.Notes,
                KolKasherGroupNumber = bllGroup.KolKasherGroupNumber
            };
        }

        /// <summary>
        /// Converts DAL Group to BLLGroupDetailsPerfect.
        /// </summary>
        private BLLGroupDetailsPerfect ToBLLGroupDetailsPerfect(Group group)
        {
            if (group == null) return null;
            var instructor = dal.Instructors.GetById(group.InstructorId);
            var branch = dal.Branches.GetById(group.BranchId);
            var course = dal.Courses.GetById(group.CourseId);

            return new BLLGroupDetailsPerfect
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
                IsActive = group.IsActive,
                Notes = group.Notes,
                KolKasherGroupNumber=group.KolKasherGroupNumber,
                LessonsCompleted = group.LessonsCompleted,
                BranchName = branch?.Name ?? string.Empty,
                CourseName = course?.CouresName ?? string.Empty,
                InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty
            };
        }

        /// <summary>
        /// Converts DAL Group to BLLGroupWithStudentsDto.
        /// </summary>
        private BLLGroupWithStudentsDto ToBLLGroupWithStudentsDto(Group group)
        {
            if (group == null) return null;
            var course = dal.Courses.GetById(group.CourseId);
            var branch = dal.Branches.GetById(group.BranchId);
            var instructor = dal.Instructors.GetById(group.InstructorId);
            var allStudents = dal.Students.Get();
            var allGroupStudents = dal.GroupStudents.Get();

            var students = allGroupStudents
                .Where(gs => gs.GroupId == group.GroupId)
                .Select(gs =>
                {
                    var student = allStudents.FirstOrDefault(st => st.Id == gs.StudentId);
                    return new StudentDto
                    {
                        StudentId = gs.StudentId,
                        StudentName = student != null ? $"{student.FirstName} {student.LastName}" : string.Empty,
                        Phone = student?.Phone,
                        City = student?.City,
                        HealthFund = student?.HealthFundForStudent != null
                            ? $"{student.HealthFundForStudent.Name} ({student.HealthFundForStudent.FundType})"
                            : string.Empty
                    };
                })
                .ToList();

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
                IsActive = group.IsActive,
                Notes = group.Notes,
                KolKasherGroupNumber=group.KolKasherGroupNumber,
                Sector = group.Sector,
                StartDate = group.StartDate,
                Schedule = $"{group.DayOfWeek} {group.Hour?.ToString("HH:mm")}",
                InstructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : string.Empty,
                Students = students
            };
        }

        /// <summary>
        /// Converts DAL Group to BLLGroupDetails.
        /// </summary>
        private BLLGroupDetails ToBLLGroupDetails(Group group)
        {
            if (group == null) return null;
            var course = dal.Courses.GetById(group.CourseId);
            var branch = dal.Branches.GetById(group.BranchId);

            return new BLLGroupDetails
            {
                GroupId = group.GroupId,
                GroupName = group.GroupName,
                DayOfWeek = group.DayOfWeek,
                CourseName = course?.CouresName,
                BranchName = branch?.Name,
                Hour = group.Hour,
                AgeRange = group.AgeRange,
                MaxStudents = group.MaxStudents,
                Sector = group.Sector,
                StartDate = group.StartDate,
                NumOfLessons = group.NumOfLessons,
                LessonsCompleted = group.LessonsCompleted,
                IsActive = group.IsActive,
                Notes = group.Notes,
                KolKasherGroupNumber=group.KolKasherGroupNumber
            };
        }

        #endregion


        /// <summary>
        /// Adds a new group and generates lessons if needed.
        /// </summary>
        public async Task CreateAsync(BLLGroup group)
        {
            Group g = ToDALGroup(group);
            int groupId = dal.Groups.Create(g);
            if (!(bool)group.IsActive)
                return;
            if (group.StartDate.HasValue)
            {
                if (group.Hour.HasValue)
                {
                    await lessonService.GenerateLessonsForGroup(
                        groupId: groupId,
                        startDate: group.StartDate.Value,
                        numOfLessons: group.NumOfLessons ?? 0,
                        dayOfWeek: group.DayOfWeek ?? "",
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
        /// Deletes a group and all related data.
        /// </summary>
        public void Delete(int id)
        {
            var groupStudents = dal.GroupStudents.Get().Where(x => x.GroupId == id);
            foreach (var item in groupStudents)
                dal.GroupStudents.Delete(item);
            var lessons = dal.Lessons.Get().Where(x => x.GroupId == id);
            foreach (var item in lessons)
                dal.Lessons.Delete(item.LessonId);
            var attendances = dal.Attendances.GetAttendanceByGroup(id);
            foreach (var item in attendances)
                dal.Attendances.Delete(item.AttendanceId);

            dal.Groups.Delete(id);
        }

        /// <summary>
        /// Returns all groups with full details.
        /// </summary>
        public List<BLLGroupDetailsPerfect> Get()
        {
            try
            {
                var groups = dal.Groups.Get();
                if (groups == null || !groups.Any())
                {
                    Console.WriteLine("No groups found.");
                    return new List<BLLGroupDetailsPerfect>();
                }
                return groups.Select(ToBLLGroupDetailsPerfect).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching groups: {ex.Message}");
                return new List<BLLGroupDetailsPerfect>();
            }
        }

        /// <summary>
        /// Returns a group by id.
        /// </summary>
        public BLLGroup GetById(int id)
        {
            var group = dal.Groups.GetById(id);
            return ToBLLGroup(group);
        }

        /// <summary>
        /// Updates group details, including generating lessons if the group becomes active,
        /// and updating future lessons if the instructor changes.
        /// </summary>
        public async Task UpdateAsync(BLLGroup group)
        {
            var existingGroup = dal.Groups.GetById(group.GroupId);
            if (existingGroup == null)
                throw new KeyNotFoundException($"Group with ID {group.GroupId} not found.");

            // Save previous state
            bool wasActive = existingGroup.IsActive ?? false;
            bool willBeActive = group.IsActive ?? false;
            int previousInstructorId = existingGroup.InstructorId;

            // Update fields
            existingGroup.CourseId = group.CourseId;
            existingGroup.BranchId = group.BranchId;
            existingGroup.AgeRange = group.AgeRange;
            existingGroup.DayOfWeek = group.DayOfWeek;
            existingGroup.GroupName = GenerateGroupName(group);
            existingGroup.Hour = group.Hour;
            existingGroup.MaxStudents = group.MaxStudents;
            existingGroup.Sector = group.Sector;
            existingGroup.InstructorId = group.InstructorId;
            existingGroup.StartDate = group.StartDate;
            existingGroup.NumOfLessons = group.NumOfLessons;
            existingGroup.IsActive = group.IsActive;
            existingGroup.LessonsCompleted = group.LessonsCompleted;
            existingGroup.Notes = group.Notes;
            existingGroup.KolKasherGroupNumber = group.KolKasherGroupNumber;

            dal.Groups.Update(existingGroup);

            // If instructor changed, update all future lessons
            if (previousInstructorId != group.InstructorId)
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                var lessons = dal.Lessons.Get()
                    .Where(l => l.GroupId == group.GroupId && l.LessonDate >= today)
                    .ToList();

                foreach (var lesson in lessons)
                {
                    lesson.InstructorId = group.InstructorId;
                    dal.Lessons.Update(lesson);
                }
            }

            // If group became active, generate lessons if needed
            if (!wasActive && willBeActive)
            {
                var existingLessons = dal.Lessons.Get().Any(l => l.GroupId == group.GroupId);
                if (!existingLessons)
                {
                    if (group.StartDate.HasValue && group.Hour.HasValue)
                    {
                        await lessonService.GenerateLessonsForGroup(
                            groupId: group.GroupId,
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
                        throw new ArgumentException("StartDate and Hour must have values when activating a group and generating lessons.");
                    }
                }
            }
        }

        /// <summary>
        /// Returns groups by course id.
        /// </summary>
        public List<BLLGroup> GetGroupsByCourseId(int courseId)
        {
            var dalGroups = dal.Groups.Get().Where(g => g.CourseId == courseId).ToList();
            var result = new List<BLLGroup>();
            foreach (var dalGroup in dalGroups)
            {
                var bllGroup = ToBLLGroup(dalGroup);
                bllGroup.ActiveStudents = GetActiveStudentsCountByGroupId(dalGroup.GroupId);
                result.Add(bllGroup);
            }
            return result;
        }

        /// <summary>
        /// Returns groups by day of week.
        /// </summary>
        public List<BLLGroupDetails> GetGroupsByDayOfWeek(string dayOfWeek)
        {
            var groups = dal.Groups.GetGroupsByDayOfWeek(dayOfWeek);
            return groups.Select(ToBLLGroupDetails).ToList();
        }

        /// <summary>
        /// Returns groups with students by day of week.
        /// </summary>
        public List<BLLGroupWithStudentsDto> GetGroupsWithStudentsByDayOfWeek(string dayOfWeek)
        {
            if (string.IsNullOrWhiteSpace(dayOfWeek))
                return new List<BLLGroupWithStudentsDto>();

            var allGroups = dal.Groups.Get()
                .Where(g => g.DayOfWeek == dayOfWeek && g.IsActive != false)
                .ToList();

            return allGroups.Select(ToBLLGroupWithStudentsDto).ToList();
        }

        /// <summary>
        /// Returns groups by instructor id.
        /// </summary>
        public List<BLLGroupDetailsPerfect> GetGroupsByInstructorId(int instructorId)
        {
            var groups = dal.Groups.GetGroupsByInstructorId(instructorId);
            return groups.Select(ToBLLGroupDetailsPerfect).ToList();
        }

        /// <summary>
        /// Returns students by group id.
        /// </summary>
        public List<BLLGroupStudentPerfect> GetStudentsByGroupId(int groupId)
        {
            var lst = dal.Groups.GetStudentsByGroupId(groupId);
            List<BLLGroupStudentPerfect> lstgp = new List<BLLGroupStudentPerfect>();
            foreach (var item in lst)
            {
                var d = dal.Groups.GetById(item.GroupId);
                var student = dal.Students.GetById(item.StudentId);

                BLLGroupStudentPerfect gspl = new BLLGroupStudentPerfect()
                {
                    StudentId = item.StudentId,
                    StudentName = student.FirstName + " " + student.LastName,
                    Student = student,
                    EnrollmentDate = item.EnrollmentDate,
                    IsActive = item.IsActive,
                    Notes = d.Notes,
                    KolKasherGroupNumber=d.KolKasherGroupNumber,
                    DayOfWeek = d.DayOfWeek,
                    Hour = d.Hour,
                    GroupName = d.GroupName,
                    BranchName = dal.Branches.GetById(d.BranchId).Name,
                    InstructorName = dal.Instructors.GetById(d.InstructorId).FirstName + " " + dal.Instructors.GetById(d.InstructorId).LastName,
                    CourseName = dal.Courses.GetById(d.CourseId).CouresName,
                    HealthFundName = student.HealthFundForStudent != null ? student.HealthFundForStudent.Name : "",
                    HealthFundPlan = student.HealthFundForStudent != null ? student.HealthFundForStudent.FundType : ""
                };
                lstgp.Add(gspl);
            }
            return lstgp;
        }

        /// <summary>
        /// Returns group with students by group id.
        /// </summary>
        public BLLGroupWithStudentsDto GetGroupWithStudentsById(int groupId)
        {
            var group = dal.Groups.GetById(groupId);
            return ToBLLGroupWithStudentsDto(group);
        }

        /// <summary>
        /// Returns all groups with students, sorted by course.
        /// </summary>
        public List<BLLGroupWithStudentsDto> GetAllGroupsWithStudentsSortedByCourse()
        {
            var groups = dal.Groups.Get();
            if (groups == null || !groups.Any())
                return new List<BLLGroupWithStudentsDto>();
            return groups.Select(ToBLLGroupWithStudentsDto)
                         .OrderBy(g => g.CourseName)
                         .ToList();
        }

        /// <summary>
        /// Returns groups with students by branch id.
        /// </summary>
        public List<BLLGroupWithStudentsDto> GetGroupsWithStudentsByBranchId(int branchId)
        {
            var branch = dal.Branches.GetById(branchId);
            if (branch == null)
                return new List<BLLGroupWithStudentsDto>();
            var allGroups = dal.Groups.Get().Where(g => g.BranchId == branchId).ToList();
            return allGroups.Select(ToBLLGroupWithStudentsDto).ToList();
        }

        /// <summary>
        /// Generates lessons for all existing groups.
        /// </summary>
        public async Task GenerateLessonsForAllExistingGroups(string createdBy)
        {
            try
            {
                var groups = dal.Groups.Get();
                foreach (var group in groups)
                {
                    if (group.StartDate == null || group.StartDate == DateOnly.MinValue)
                        continue;
                    var existingLessons = dal.Lessons.Get()?.Where(l => l.GroupId == group.GroupId).ToList();
                    if (existingLessons != null && existingLessons.Any())
                        continue;
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
        /// Returns full group details including lessons and students.
        /// </summary>
        public BLLGroupDetailsDto GetGroupDetails(int groupId)
        {
            var group = dal.Groups.GetByIdWithIncludes(groupId);
            if (group == null)
                return null;

            var students = group.GroupStudents.Select(gs => gs.Student).ToList();
            var lessons = group.Lessons?.ToList() ?? new List<Lesson>();
            var instructor = group.Instructor;

            UpdateLessonStatusesByDate(lessons);

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
                IsActive = group.IsActive,
                Notes = group.Notes,
                Branch = group.Branch,
                Course = group.Course,
                Instructor = instructor,
                Students = students,
                Lessons = lessons
            };
        }

        /// <summary>
        /// Returns the number of active students in a group.
        /// </summary>
        public int GetActiveStudentsCountByGroupId(int groupId)
        {
            var groupStudents = dal.GroupStudents.Get().Where(gs => gs.GroupId == groupId);
            return groupStudents.Count(gs => gs.IsActive == 1);
        }

        /// <summary>
        /// Updates lesson statuses by date.
        /// </summary>
        private void UpdateLessonStatusesByDate(List<Lesson> lessons)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            foreach (var lesson in lessons)
            {
                if (lesson.Status != "canceled" && lesson.Status != "completion")
                {
                    if (lesson.LessonDate < now)
                        lesson.Status = "completed";
                    else if (lesson.LessonDate > now)
                        lesson.Status = "future";
                    else lesson.Status = "today";
                }
            }
        }

        /// <summary>
        /// Generates a group name based on group fields.
        /// </summary>
        private string GenerateGroupName(BLLGroup group)
        {
            var branch = dal.Branches.GetById(group.BranchId);
            var instructor = dal.Instructors.GetById(group.InstructorId);

            string branchName = branch?.Name ?? "";
            string day = group.DayOfWeek ?? "";
            string hour = group.Hour?.ToString("HH:mm") ?? "";
            string instructorName = instructor != null ? $"{instructor.FirstName} {instructor.LastName}" : "";
            string age = group.AgeRange ?? "";
            string sector = group.Sector ?? "";

            return $"{branchName} {day} {hour} {instructorName} {age} {sector}".Trim();
        }

        #region FindBestGroupsForStudent

        /// <summary>
        /// Finds the best groups for a student.
        /// </summary>
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
                .OrderByDescending(x => x.MatchScore)
                .ThenBy(x => x.Priority)
                .ThenByDescending(x => x.Group.MaxStudents)
                .ThenBy(x => x.Group.StartDate)
                .Take(maxResults)
                .ToList();

            var result = new List<BLLGroupDetailsPerfect>();

            foreach (var item in eligibleGroups)
            {
                var group = item.Group;
                var branch = dal.Branches.GetById(group.BranchId);
                var course = dal.Courses.GetById(group.CourseId);
                var instructor = dal.Instructors.GetById(group.InstructorId);

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
                    MatchScore = item.MatchScore,
                    MatchReasons = GenerateMatchReasons(group, student, branch, course)
                };

                result.Add(bllGroup);
            }

            return result;
        }

        /// <summary>
        /// Finds the best group for a student.
        /// </summary>
        public BLLGroupDetailsPerfect FindBestGroupForStudent(int studentId)
        {
            var bestGroups = FindBestGroupsForStudent(studentId, 1);
            return bestGroups.FirstOrDefault() ?? new BLLGroupDetailsPerfect();
        }

        private int CalculateMatchScore(Group group, Student student)
        {
            int score = 50;
            if (group.MaxStudents.HasValue && group.MaxStudents > 0)
                score += Math.Min(group.MaxStudents.Value * 2, 20);
            if (!string.IsNullOrEmpty(group.Sector) && group.Sector == student.Sector)
                score += 15;
            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart >= 0 && daysUntilStart <= 30)
                    score += 10;
            }
            if (group.NumOfLessons.HasValue && group.LessonsCompleted.HasValue)
            {
                var remainingLessons = group.NumOfLessons.Value - group.LessonsCompleted.Value;
                if (remainingLessons > group.NumOfLessons.Value * 0.7)
                    score += 5;
            }
            return Math.Min(score, 100);
        }

        private int CalculatePriority(Group group, Student student)
        {
            int priority = 0;
            if (string.IsNullOrEmpty(group.Sector) || group.Sector != student.Sector)
                priority += 10;
            if (!group.MaxStudents.HasValue || group.MaxStudents <= 0)
                priority += 50;
            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart > 60)
                    priority += 20;
            }
            return priority;
        }

        private List<string> GenerateMatchReasons(Group group, Student student, Branch branch, Course course)
        {
            var reasons = new List<string>();
            if (group.MaxStudents.HasValue && group.MaxStudents > 0)
                reasons.Add($"{group.MaxStudents} מקומות פנויים");
            if (!string.IsNullOrEmpty(group.Sector) && group.Sector == student.Sector)
                reasons.Add($"התאמת מגזר - {group.Sector}");
            if (branch != null && !string.IsNullOrEmpty(branch.City))
                reasons.Add($"סניף ב{branch.City}");
            if (group.StartDate.HasValue)
            {
                var daysUntilStart = (group.StartDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days;
                if (daysUntilStart >= 0 && daysUntilStart <= 30)
                    reasons.Add("מתחיל בקרוב");
            }
            if (!string.IsNullOrEmpty(group.AgeRange))
                reasons.Add($"מתאים לגיל {group.AgeRange}");
            if (course != null && !string.IsNullOrEmpty(course.CouresName))
                reasons.Add($"חוג {course.CouresName}");
            return reasons.Count > 0 ? reasons : new List<string> { "קבוצה זמינה" };
        }

        private bool IsStudentInAgeRange(int age, string? ageRange)
        {
            if (string.IsNullOrEmpty(ageRange))
                return true;
            var parts = ageRange.Split('-');
            if (parts.Length != 2)
                return false;
            if (int.TryParse(parts[0], out int minAge) && int.TryParse(parts[1], out int maxAge))
                return age >= minAge && age <= maxAge;
            return false;
        }

        #endregion
    }
}

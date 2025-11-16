using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLGroupStudentService : IBLLGroupStudent
    {
        private readonly IDAL dal;
        public BLLGroupStudentService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLGroupStudent groupStudent)
        {
            GroupStudent g = new GroupStudent()
            { 
                GroupId = groupStudent.GroupId,
                StudentId = groupStudent.StudentId,
                IsActive = groupStudent.IsActive == true || dal.Students.GetById(groupStudent.StudentId).Status == "פעיל",
                EnrollmentDate = groupStudent.EnrollmentDate ?? DateOnly.FromDateTime(DateTime.Now)
            };
            dal.GroupStudents.Create(g);

            var gl = dal.Groups.Get().ToList().Find(x => x.GroupId == groupStudent.GroupId);
            if (gl != null)
            {
                gl.MaxStudents = (gl.MaxStudents ?? 0) - 1;
                dal.Groups.Update(gl);
            }
            var branch =dal.Branches.Get().ToList().Find(x => x.BranchId == gl?.BranchId);
            if (branch != null)
            {
                branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) + 1;
                dal.Branches.Update(branch);
            }
            // הוספת שיעורים לנוכחות אם התלמיד פעיל ותאריך ההתחלה הוא בעבר
            if (g.IsActive == true && g.EnrollmentDate < DateOnly.FromDateTime(DateTime.Now))
            {
                var lessons = dal.Attendances.GetByGroupAndDateRange(
                    groupStudent.GroupId,
                    g.EnrollmentDate.Value,
                    DateOnly.FromDateTime(DateTime.Now)
                );

                foreach (var lesson in lessons)
                {
                    var existingAttendance = dal.Attendances.GetAttendanceByGroupAndDate(groupStudent.GroupId, (DateOnly)lesson.Date)
                        .FirstOrDefault(a => a.StudentId == groupStudent.StudentId);

                    if (existingAttendance == null)
                    {
                        dal.Attendances.Create(new Attendance
                        {
                            GroupId = groupStudent.GroupId,
                            StudentId = groupStudent.StudentId,
                            Date = lesson.Date,
                            WasPresent = true
                        });
                    }
                }
            }

        }

        public void Delete(int id)
        {
            var groupStudent = dal.GroupStudents.GetById(id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            dal.GroupStudents.Delete(groupStudent);

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

        public void DeleteByGsId(int id)
       
        {
            var groupStudent = GetByGsId(id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {id} not found.");
            }

            dal.GroupStudents.Delete(id);

            var group = dal.Groups.GetById(groupStudent.GroupId);
            if (group != null)
            {
                group.MaxStudents = (group.MaxStudents ?? 0) + 1;
                dal.Groups.Update(group);
            }
            var branch = dal.Branches.Get().ToList().Find(x => x.BranchId == group?.BranchId);
            if (branch != null)
            {
                branch.MaxGroupSize = (branch.MaxGroupSize ?? 0) -1;
                dal.Branches.Update(branch);
            }
        }

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
                    IsActive = gs.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group students: {ex.Message}");
                return new List<BLLGroupStudent>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

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
                IsActive = groupStudent.IsActive
            };
        }
        public BLLGroupStudent GetByGsId(int id)
        {
            var groupStudent = dal.GroupStudents.Get().SingleOrDefault(x=>x.GroupStudentId==id);
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
                IsActive = groupStudent.IsActive
            };
        }
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
                        GroupId=item.GroupId,
                        StudentId = item.StudentId,
                        StudentName = $"{dal.Students.GetById(item.StudentId).FirstName} {dal.Students.GetById(item.StudentId).LastName}",
                        Student=dal.Students.GetById(item.StudentId),
                        EnrollmentDate = item.EnrollmentDate,
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
                        IsActive = gs.IsActive,
                        DayOfWeek = group?.DayOfWeek??string.Empty,
                        Hour = group?.Hour,
                        GroupName = group?.GroupName?? string.Empty,
                        BranchName = group != null ? dal.Branches.GetById(group.BranchId)?.Name : "",
                        InstructorName = group != null ? $"{dal.Instructors.GetById(group.InstructorId)?.FirstName} {dal.Instructors.GetById(group.InstructorId)?.LastName}" : "",
                        CourseName = group != null ? dal.Courses.GetById(group.CourseId)?.CouresName : "",
                        AgeRange=group?.AgeRange??string.Empty,
                        LessonsCompleted=group?.LessonsCompleted,
                        MaxStudents=group?.MaxStudents,
                        NumOfLessons=group?.NumOfLessons
                   
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<BLLGroupStudentPerfect>();
            }
        }
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
                Email = i.Email??="",
                City = i.City??="",
                Sector = i.Sector ??= ""
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
                IsActive = gs.IsActive
            }).ToList();
        }

        public void Update(BLLGroupStudentSecondly groupStudent)
        {
            var existingGroupStudent = dal.GroupStudents.GetById(groupStudent.GroupStudentId);
            if (existingGroupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {groupStudent.GroupStudentId} not found.");
            }

            existingGroupStudent.GroupId = dal.Groups.Get()
                .Where(x => x.GroupName == groupStudent.GroupName)
                .Select(x => x.GroupId)
                .FirstOrDefault();
            existingGroupStudent.StudentId = groupStudent.StudentId;
            existingGroupStudent.EnrollmentDate = groupStudent.EnrollmentDate;
            existingGroupStudent.IsActive = groupStudent.IsActive;

            dal.GroupStudents.Update(existingGroupStudent);
        }
    }
}

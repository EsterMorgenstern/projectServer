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
                IsActive = false,
                EnrollmentDate = DateOnly.FromDateTime(DateTime.Now)
            };
            dal.GroupStudents.Create(g);

            var gl = dal.Groups.Get().ToList().Find(x => x.GroupId == groupStudent.GroupId);
            if (gl != null)
            {
                gl.MaxStudents = (gl.MaxStudents ?? 0) - 1;
                dal.Groups.Update(gl);
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
                        StudentId = item.StudentId,
                        StudentName = $"{dal.Students.GetById(item.StudentId).FirstName} {dal.Students.GetById(item.StudentId).LastName}",
                        EnrollmentDate = item.EnrollmentDate,
                        IsActive = item.IsActive,
                        DayOfWeek = d.DayOfWeek,
                        Hour = d.Hour,
                        GroupName = d.GroupName,
                        BranchName = dal.Branches.GetById(d.BranchId).Name,
                        InstructorName = $"{dal.Instructors.GetById(d.InstructorId).FirstName} {dal.Instructors.GetById(d.InstructorId).LastName}",
                        CourseName = dal.Courses.GetById(d.CourseId).CouresName
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching group students for student ID {id}: {ex.Message}");
                return new List<BLLGroupStudentPerfect>(); // מחזיר מערך ריק במקרה של שגיאה
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

        public void Update(BLLGroupStudent groupStudent)
        {
            var existingGroupStudent = dal.GroupStudents.GetById(groupStudent.GroupStudentId);
            if (existingGroupStudent == null)
            {
                throw new KeyNotFoundException($"GroupStudent with ID {groupStudent.GroupStudentId} not found.");
            }

            existingGroupStudent.GroupId = groupStudent.GroupId;
            existingGroupStudent.StudentId = groupStudent.StudentId;
            existingGroupStudent.EnrollmentDate = groupStudent.EnrollmentDate;
            existingGroupStudent.IsActive = groupStudent.IsActive;

            dal.GroupStudents.Update(existingGroupStudent);
        }
    }
}

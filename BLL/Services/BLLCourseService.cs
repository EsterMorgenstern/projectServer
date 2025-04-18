﻿using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLCourseService : IBLLCourse
    {
        IDAL dal;
        public BLLCourseService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLCourse course)
        {
            Course c = new Course()
            {
                CourseId = course.CourseId,
                CouresName = course.CourseName,
                InstructorId = course.InstructorId,
                MaxNumOfStudents = course.MaxNumOfStudent,
                NumOfStudents = course.NumOfStudents,
                StartDate = course.StartDate
            };
            dal.Courses.Create(c);
        }

        public void Delete(BLLCourse course)
        {
            throw new NotImplementedException();
        }

        public List<BLLCourse> Get()
        {
            return dal.Courses.Get().Select(c => new BLLCourse()
            {
                CourseId = c.CourseId,
                CourseName = c.CouresName,
                InstructorId = c.InstructorId,
                MaxNumOfStudent = c.MaxNumOfStudents ?? 0,
                NumOfStudents = c.NumOfStudents ?? 0,
                StartDate = c.StartDate
            }).ToList();
        }

        public BLLCourse? GetById(int id)
        {
            Course c = dal.Courses.GetById(id);
            BLLCourse blc = new BLLCourse()
            {
                CourseId = c.CourseId,
                CourseName = c.CouresName,
                InstructorId = c.InstructorId,
                MaxNumOfStudent = c.MaxNumOfStudents ?? 0,
                NumOfStudents = c.NumOfStudents ?? 0,
                StartDate = c.StartDate
            };
            return blc;
        }

        public void Update(BLLCourse course)
        {
            Course c = dal.Courses.GetById(course.CourseId);
            if (c != null)
            {
                c.CourseId = course.CourseId;
                c.CouresName = course.CourseName;
                c.InstructorId = course.InstructorId;
                c.MaxNumOfStudents = course.MaxNumOfStudent;
                c.NumOfStudents = course.NumOfStudents;
                c.StartDate = course.StartDate;
                dal.Courses.Update(c);
            }
            else
            {
                throw new KeyNotFoundException($"Course with id {course.CourseId} not found.");
            }   
        }
    }
}

﻿using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLCourseService : IBLLCourse
    {
        private readonly IDAL dal;
        public BLLCourseService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLCourse course)
        {
            Course c = new Course()
            {
                CourseId = course.CourseId,
                CouresName = course.CouresName,
                Description = course.Description
            };
            dal.Courses.Create(c);
        }

        public void Delete(int courseId)
        {
            var groups = dal.Groups.Get().Where(x => x.CourseId == courseId);
            foreach (var group in groups)
            {
                dal.Groups.Delete(group.GroupId);
            }
            dal.Courses.Delete(courseId);
        }

        public List<BLLCourse> Get()
        {
            try
            {
                var courses = dal.Courses.Get();
                if (courses == null || !courses.Any())
                {
                    Console.WriteLine("No courses found.");
                    return new List<BLLCourse>(); // מחזיר מערך ריק
                }

                return courses.Select(c => new BLLCourse()
                {
                    CourseId = c.CourseId,
                    CouresName = c.CouresName,
                    Description = c.Description
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching courses: {ex.Message}");
                return new List<BLLCourse>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }

        public BLLCourse GetById(int id)
        {
            try
            {
                var course = dal.Courses.GetById(id);
                if (course != null)
                {
                    return new BLLCourse()
                    {
                        CourseId = course.CourseId,
                        CouresName = course.CouresName,
                        Description = course.Description
                    };
                }

                Console.WriteLine($"Course with ID {id} not found.");
                return new BLLCourse()
                {
                    CourseId = 0,
                    CouresName = string.Empty,
                    Description = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching course with ID {id}: {ex.Message}");
                return new BLLCourse()
                {
                    CourseId = 0,
                    CouresName = string.Empty,
                    Description = string.Empty
                };
            }
        }

        public void Update(BLLCourse course)
        {
            Course c = dal.Courses.GetById(course.CourseId);
            if (c != null)
            {
                c.CourseId = course.CourseId;
                c.CouresName = course.CouresName;
                c.Description = course.Description;

                dal.Courses.Update(c);
            }
            else
            {
                throw new KeyNotFoundException($"Course with id {course.CourseId} not found.");
            }
        }
    }
}

﻿using System.Net;
using BLL.Api;
using BLL.Models;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLBranchService : IBLLBranch
    {
        private readonly IDAL dal;
        public BLLBranchService(IDAL dal)
        {
            this.dal = dal;
        }
        public List<BLLBranch> Get()
        {
            try
            {
                var branches = dal.Branches.Get();
                if (branches == null || !branches.Any())
                {
                    Console.WriteLine("No branches found.");
                    return new List<BLLBranch>(); // מחזיר מערך ריק
                }

                return branches.Select(b => new BLLBranch()
                {
                    BranchId = b.BranchId,
                    CourseId = b.CourseId,
                    Name = b.Name,
                    Address = b.Address,
                    MaxGroupSize = b.MaxGroupSize,
                    City = b.City
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching branches: {ex.Message}");
                return new List<BLLBranch>(); // מחזיר מערך ריק במקרה של שגיאה
            }
        }
        public void Create(BLLBranch branch)
        {
            Branch b = new Branch()
            {
                BranchId = branch.BranchId,
                CourseId = branch.CourseId,
                Name = branch.Name,
                Address = branch.Address,
                MaxGroupSize = branch.MaxGroupSize,
                City= branch.City
            };
            dal.Branches.Create(b);
        }
        public BLLBranch GetById(int id)
        {
            try
            {
                var branch = dal.Branches.GetById(id);
                if (branch != null)
                {
                    return new BLLBranch()
                    {
                        BranchId = branch.BranchId,
                        CourseId = branch.CourseId,
                        Name = branch.Name,
                        Address = branch.Address,
                        MaxGroupSize = branch.MaxGroupSize,
                        City = branch.City
                    };
                }

                Console.WriteLine($"Branch with ID {id} not found.");
                return new BLLBranch()
                {
                    BranchId = 0,
                    CourseId = 0,
                    Name = string.Empty,
                    Address = string.Empty,
                    MaxGroupSize = null,
                    City = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching branch with ID {id}: {ex.Message}");
                return new BLLBranch()
                {
                    BranchId = 0,
                    CourseId = 0,
                    Name = string.Empty,
                    Address = string.Empty,
                    MaxGroupSize = null,
                    City = string.Empty
                };
            }
        }
       
        public void Delete(int id)
        {
            var groups = dal.Groups.Get().Where(x => x.BranchId == id);
            foreach (var item in groups)
            {
                var groupStudents = dal.GroupStudents.Get().Where(x => x.GroupId == item.GroupId);
                foreach (var item1 in groupStudents)
                {
                    dal.GroupStudents.Delete(item1);
                }
                var lessonCancel = dal.LessonCancellations.Get().Where(x => x.GroupId == item.GroupId);
                foreach (var item2 in lessonCancel)
                {
                    dal.LessonCancellations.Delete(item2.Id);
                }
                var attendances = dal.Attendances.GetAttendanceByGroup(item.GroupId);
                foreach (var item3 in attendances)
                {
                    dal.Attendances.Delete(item3.AttendanceId);
                }


                dal.Groups.Delete(item.GroupId);
            }
           
            
            dal.Branches.Delete(id);
        }
        public void Update(BLLBranch branch)
        {
            Branch b = dal.Branches.GetById(branch.BranchId);
            if (b != null)
            {
                b.BranchId = branch.BranchId;
                b.CourseId = branch.CourseId;
                b.Name = branch.Name;
                b.Address = branch.Address;
                b.MaxGroupSize = branch.MaxGroupSize;
                b.City = branch.City;   

                dal.Branches.Update(b);
            }
            else
            {
                throw new KeyNotFoundException($"Branch with id {branch.BranchId} not found.");
            }
        }
    }







}

﻿using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALInstructorService : IDALInstructor
    {
        dbcontext dbcontext;
        public DALInstructorService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Instructor instructor)
        {
            if (string.IsNullOrWhiteSpace(instructor.FirstName) || string.IsNullOrWhiteSpace(instructor.LastName))
            {
                throw new ArgumentException("Instructor's first and last name cannot be empty.");
            }
            dbcontext.Instructors.Add(instructor);
            dbcontext.SaveChanges();
        }

        public void Delete(int id)
        {
            var instructor = dbcontext.Instructors.SingleOrDefault(x => x.Id == id);
            if (instructor == null)
            {
                throw new KeyNotFoundException($"Instructor with ID {id} not found.");
            }

            dbcontext.Instructors.Remove(instructor);
            dbcontext.SaveChanges();
        }

        public List<Instructor> Get()
        {
            if(dbcontext.Instructors !=null)
                return dbcontext.Instructors.ToList();
            else
                throw new Exception("No instructors found.");
        }

        public Instructor GetById(int id) 
        {
            var instructor = dbcontext.Instructors.SingleOrDefault(x => x.Id == id);
            if (instructor == null)
            {
                throw new KeyNotFoundException($"Instructor with ID {id} not found.");
            }
            return instructor;
        }



        public void Update(Instructor instructor)
        {
            dbcontext.Instructors.Update(instructor);
            dbcontext.SaveChanges();
        }

    }
}

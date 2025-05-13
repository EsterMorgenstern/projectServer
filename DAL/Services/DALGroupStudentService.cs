using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALGroupStudentService:IDALGroupStudent
    {
        dbcontext dbcontext;
        public DALGroupStudentService(dbcontext data)
        {
            dbcontext = data;
        }
        public void Create(GroupStudent groupStudent)
        {
            dbcontext.GroupStudents.Add(groupStudent);
            dbcontext.SaveChanges();
        }
        public void Delete(int id)
        {
            var trackedGroupStudent = dbcontext.GroupStudents.Find(id);
            if (trackedGroupStudent != null)
            {
                dbcontext.GroupStudents.Remove(trackedGroupStudent);
                dbcontext.SaveChanges();
            }
        }

        public void Delete(GroupStudent groupStudent)
        {
            throw new NotImplementedException();
        }

        public List<GroupStudent> Get()
        {
            return dbcontext.GroupStudents.ToList();
        }
        public GroupStudent GetById(int id)
        {
            var groupStudent = dbcontext.GroupStudents.SingleOrDefault(x => x.GroupId == id);
            if (groupStudent == null)
            {
                throw new KeyNotFoundException($"Group with ID {id} not found.");
            }
            return groupStudent;
        }
        public void Update(GroupStudent groupStudent)
        {
            dbcontext.GroupStudents.Update(groupStudent);
            dbcontext.SaveChanges();
        }   
    }
}

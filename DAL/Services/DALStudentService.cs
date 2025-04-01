using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Models;
using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALStudentService : IDALStudent
    {
        dbcontext dbcontext;
        public DALStudentService(dbcontext data)
        {
               dbcontext = data;
        }

        public void Create(Student item)
        {
            dbcontext.Students.Add(item);
            dbcontext.SaveChanges();
        }

        public List<Student> Get()
        {
            return dbcontext.Students.ToList();
        }
       
    }
}

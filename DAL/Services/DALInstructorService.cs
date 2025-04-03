using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class DALInstructorService:IDALInstructor
    {
        dbcontext dbcontext;
        public DALInstructorService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Instructor instructor)
        {
            throw new NotImplementedException();
        }

        public void Delete(Instructor instructor)
        {
            throw new NotImplementedException();
        }

        public List<Instructor> Get()
        {
            throw new NotImplementedException();
        }

        public Instructor GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Instructor instructor)
        {
            throw new NotImplementedException();
        }
    }
}

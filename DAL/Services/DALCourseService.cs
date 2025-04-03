using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Api;


namespace DAL.Services
{
    public class DALCourseService:IDALCourse
    {
        dbcontext dbcontext;
        public DALCourseService(dbcontext data)
        {
            dbcontext = data;
        }
        public void Create(Course course)
        {
            throw new NotImplementedException();
        }

        public void Delete(Course course)
        {
            throw new NotImplementedException();
        }

        public List<Course> Get()
        {
            throw new NotImplementedException();
        }

        public Course GetById(int id)
        {
            throw new NotImplementedException();
        }
        public void Update(Course course)
        {
            throw new NotImplementedException();
        }
    }
}

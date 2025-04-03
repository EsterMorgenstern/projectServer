using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Services;

namespace DAL
{
    public class DALManager:IDAL
    {
        dbcontext data = new dbcontext();

        public DALManager()
        {
            Students = new DALStudentService(data);
            Instructors = new DALInstructorService(data);
            Courses =new DALCourseService(data);     
        }

        public IDALStudent Students { get; }

        public IDALInstructor Instructors { get; }

        public IDALCourse Courses { get; }

    }
}

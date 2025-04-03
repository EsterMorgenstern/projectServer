using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Api;
using BLL.Services;
using DAL;
using DAL.Api;

namespace BLL
{
    public class BLLManager:IBLL
    {
        public IBLLStudent Students {  get;  } 
        public IBLLInstructor Instructors {  get; }
        public IBLLCourse Courses {  get; }

       

        public BLLManager()
        {
            IDAL dal = new DALManager(); 
            Students = new BLLStudentService(dal);
            Instructors = new BLLInstructorService(dal);
            Courses=new BLLCourseService(dal);  
            
        }
    }
}

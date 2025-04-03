using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Api
{
    public interface IBLL
    {
        public IBLLCourse Students { get; }
        public IBLLInstructor Instructors { get; }
        public IBLLCourse Courses { get; }
       
    }
}

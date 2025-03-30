using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Api
{
    public interface IDAL
    {
        public IDALStudent Students { get;}
        public IDALInstructor Instructors{ get; }
        public IDALCourse Courses { get; }
    }
}

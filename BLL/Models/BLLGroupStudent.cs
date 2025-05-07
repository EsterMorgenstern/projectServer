using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLLGroupStudent
    {
        public int GroupStudentId { get; set; }

        public int StudentId { get; set; }

        public int GroupId { get; set; }

        public DateOnly? EnrollmentDate { get; set; }

        public bool? IsActive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime RegistrationDate { get; set; }

        //public DateTime EndDate { get; set; }
        //public string Status { get; set; } // "Active", "Completed", "Dropped"
        //public string Grade { get; set; } // "A", "B", "C", "D", "F"
        //public string Feedback { get; set; } // Feedback from the instructor
    }
}

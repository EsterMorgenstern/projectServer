using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLLGroup
    {
        public int GroupId { get; set; }

        public int BranchId { get; set; }

        public int CourseId { get; set; }

        public int InstructorId { get; set; }

        public string GroupName { get; set; } = null!;

        public string DayOfWeek { get; set; } = null!;

        public TimeOnly? Hour { get; set; }

        public string? AgeRange { get; set; }

        public int? MaxStudents { get; set; }

        public string? Sector { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? NumOfLessons { get; set; }
    }
}

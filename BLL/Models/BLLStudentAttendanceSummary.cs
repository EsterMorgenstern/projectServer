using System;
using System.Collections.Generic;

namespace BLL.Models
{
    public class BLLStudentAttendanceSummary
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalLessons { get; set; }
        public int AttendedLessons { get; set; }
        public int AbsentLessons { get; set; }
        public double AttendanceRate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}

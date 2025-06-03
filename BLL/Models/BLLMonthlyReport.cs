using System;
using System.Collections.Generic;

namespace BLL.Models
{
    public class BLLMonthlyReport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public BLLOverallStatistics OverallStatistics { get; set; } = new();
        public List<BLLGroupReport> Groups { get; set; } = new();
    }

    public class BLLOverallStatistics
    {
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; }
        public int TotalGroups { get; set; }
        public double OverallAttendanceRate { get; set; }
    }

    public class BLLGroupReport
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; }
        public double AverageAttendanceRate { get; set; }
    }
}

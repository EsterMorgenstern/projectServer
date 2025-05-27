namespace BLL.Models
{
    public class BLLAttendanceStatistics
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; }
        public double AverageAttendanceRate { get; set; }
        public List<BLLStudentAttendanceStats> StudentStats { get; set; } = new();
    }
    public class BLLStudentAttendanceStats
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalLessons { get; set; }
        public int AttendedLessons { get; set; }
        public double AttendanceRate { get; set; }
    }
}

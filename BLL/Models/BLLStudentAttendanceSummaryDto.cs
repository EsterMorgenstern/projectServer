namespace BLL.Models
{
    public class BLLStudentAttendanceSummaryDto
    {
        public int StudentId { get; set; }
        public int TotalLessons { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendanceRate { get; set; } 
    }
}

using System;

namespace BLL.Models
{
    public class BLLStudentAttendanceHistory
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly? LessonTime { get; set; }
        public bool IsPresent { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}

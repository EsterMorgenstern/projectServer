namespace BLL.Models
{
    public class BLLAttendance
    {
        public int AttendanceId { get; set; }
        public int LessonId { get; set; }
        public int StudentId { get; set; }
        public bool WasPresent { get; set; }
        public byte StatusReport { get; set; }
        public int? HealthFundReport { get; set; }
        public DateOnly? DateReport { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
    }

}

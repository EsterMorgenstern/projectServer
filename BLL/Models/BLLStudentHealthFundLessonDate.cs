public class BLLStudentHealthFundLessonDate
{
    public int AttendanceId { get; set; }
    public int LessonId { get; set; }
    public DateOnly LessonDate { get; set; }
    public TimeOnly? LessonHour { get; set; }
    public byte StatusReport { get; set; }
    public DateOnly? DateReport { get; set; }
}
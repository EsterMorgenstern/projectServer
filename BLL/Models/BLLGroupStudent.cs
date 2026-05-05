namespace BLL.Models
{
    public class BLLGroupStudent
    {
        public int GroupStudentId { get; set; }
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public DateOnly? TrialDate { get; set; }
        public byte? IsActive { get; set; }
    }
}

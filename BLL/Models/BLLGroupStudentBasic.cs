namespace BLL.Models
{
    public class BLLGroupStudentBasic
    {
        public int GroupStudentId { get; set; }
        public int StudentId { get; set; }
        public string? StudentFirstName { get; set; }
        public string? StudentLastName { get; set; }
        public string? GroupName { get; set; }
        public byte? IsActive { get; set; }
        public DateOnly? EnrollmentDate { get; set; }
        public DateOnly? TrialDate { get; set; }
    }
}
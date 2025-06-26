
namespace BLL.Models
{
    public class BLLGroupDetails
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string DayOfWeek { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public TimeOnly? Hour { get; set; }
        public string? AgeRange { get; set; }
        public int? MaxStudents { get; set; }
        public string? Sector { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? NumOfLessons { get; set; }
    }
}

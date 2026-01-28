
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
        public int? LessonsCompleted { get; set; }
        public bool? IsActive { get; set; }
      
    }
    public class BLLGroupDetailsPerfect
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string DayOfWeek { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int CourseId{ get; set; } 
        public string BranchName { get; set; } = null!;
        public int BranchId { get; set; }
        public int InstructorId { get; set; }
        public TimeOnly? Hour { get; set; }
        public string? AgeRange { get; set; }
        public int? MaxStudents { get; set; }
        public string? Sector { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? NumOfLessons { get; set; }
        public int? LessonsCompleted { get; set; }
        public bool? IsActive { get; set; }

        // שדות חדשים
        public string InstructorName { get; set; } = string.Empty;
        public string BranchCity { get; set; } = string.Empty;
        public string BranchAddress { get; set; } = string.Empty;
        public int MatchScore { get; set; }
        public List<string> MatchReasons { get; set; } = new List<string>();

        // שדות מחושבים
        public int AvailableSpots => MaxStudents ?? 0;
        public bool HasAvailableSpots => MaxStudents.HasValue && MaxStudents > 0;
        public string Schedule => $"{DayOfWeek} {Hour?.ToString("HH:mm")}";
        public string Location => $"{BranchName}, {BranchCity}";
        public int RemainingLessons => (NumOfLessons ?? 0) - (LessonsCompleted ?? 0);
    }

}

using DAL.Models;

namespace BLL.Models
{
    public class BLLGroupDetailsDto
    {
        public int GroupId { get; set; }
        public int BranchId { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public string? GroupName { get; set; }
        public string? DayOfWeek { get; set; }
        public TimeOnly? Hour { get; set; }
        public string? AgeRange { get; set; }
        public int? MaxStudents { get; set; }
        public string? Sector { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? NumOfLessons { get; set; }
        public int? LessonsCompleted { get; set; }
        public bool? IsActive { get; set; }
        public Branch? Branch { get; set; }
        public Course? Course { get; set; }
        public Instructor? Instructor { get; set; }
        public List<Student>? Students { get; set; }
        public List<Lesson>? Lessons { get; set; }
    }
}
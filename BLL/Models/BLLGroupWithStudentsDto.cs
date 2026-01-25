namespace BLL.Services
{
    public class BLLGroupWithStudentsDto
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? CourseName { get; set; }
        public string? BranchName { get; set; }
        public string? Schedule { get; set; }
        public string? AgeRange { get; set; }
        public int? MaxStudents { get; set; }
        public string? Sector { get; set; }
        public DateOnly? StartDate { get; set; }
        public int? NumOfLessons { get; set; }
        public bool? IsActive { get; set; }
        public int? LessonsCompleted { get; set; }
        public string? InstructorName { get; set; }
        public List<StudentDto>? Students { get; set; }
    }

    public class StudentDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? HealthFund { get; set; }
    }
}

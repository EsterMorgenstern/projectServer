namespace BLL.Models
{
    public class BLLLessonCancellations
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
        public DateOnly Created_at { get; set; }
        public string? Created_by { get; set; }

    }

    public class BLLLessonCancellationsDetails
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? CourseName { get; set; }
        public string? BranchName { get; set; }
        public TimeOnly? Hour { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
        public DateOnly Created_at { get; set; }
        public string? Created_by { get; set; }
    }


}

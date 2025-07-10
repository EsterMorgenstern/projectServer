namespace DAL.Models
{
    public class LessonCancellations
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateOnly Date { get; set; }
        public string? Reason { get; set; }
        public DateOnly Created_at { get; set; }
        public string? Created_by { get; set; }



    }
}

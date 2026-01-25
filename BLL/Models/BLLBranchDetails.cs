namespace BLL.Models
{
    public class BLLBranchDetails
    {
        public int BranchId { get; set; }
        public int CourseId { get; set; }

        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public int? MaxGroupSize { get; set; }
        public string? City { get; set; }
        public int? ActiveStudentsCount { get; set; }

    }
}

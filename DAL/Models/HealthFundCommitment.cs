namespace DAL.Models
{
    public class HealthFundCommitment
    {
        public int Id { get; set; }
        public int StudentHealthFundId { get; set; }
        public string CommitmentNumber { get; set; } = string.Empty;
        public int? CommitmentTreatments { get; set; }
        public int UsedTreatments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FilePath { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        public StudentHealthFund? StudentHealthFund { get; set; }
    }
}
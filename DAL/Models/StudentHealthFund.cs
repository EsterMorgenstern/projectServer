namespace DAL.Models
{
    public class StudentHealthFund
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }

        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? EndDate { get; set; }

        public Student? Student { get; set; }
        public HealthFund? HealthFund { get; set; }

        public ICollection<HealthFundCommitment>? Commitments { get; set; }
    }
}
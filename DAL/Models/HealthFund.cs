namespace DAL.Models
{
    public class HealthFund
    {
        public int HealthFundId { get; set; }
        public string? Name { get; set; }
        public string? FundType { get; set; }
        public int? MaxTreatmentsPerYear { get; set; }
        public decimal? PricePerLesson { get; set; }
        public decimal? MonthlyPrice { get; set; }
        public bool RequiresReferral { get; set; }
        public bool RequiresCommitment { get; set; }
        public bool IsActive { get; set; }
        public int? ValidUntilAge { get; set; }
        public string? EligibilityDetails { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
        public ICollection<StudentHealthFund>? StudentHealthFunds { get; set; }
    }
}
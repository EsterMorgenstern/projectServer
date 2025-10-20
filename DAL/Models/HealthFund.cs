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
        public bool IsActive { get; set; } = true;

        public ICollection<StudentHealthFund> StudentHealthFunds { get; set; }
    }
}
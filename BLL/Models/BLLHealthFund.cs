namespace BLL.Models
{
    public class BLLHealthFund
    {
        public int HealthFundId { get; set; }
        public string? Name { get; set; }           // מכבי, כללית, מאוחדת, לאומית
        public string? FundType { get; set; }       // מכבי זהב, מכבי שלי, מאוחדת עדיף וכו’
        public int? MaxTreatmentsPerYear { get; set; }
        public decimal? PricePerLesson { get; set; }
        public decimal? MonthlyPrice { get; set; }
        public bool RequiresReferral { get; set; }
        public bool RequiresCommitment { get; set; }
        public bool IsActive { get; set; } = true;
        public int ValidUntilAge { get; set; } 
        public string? EligibilityDetails { get; set; } 

    }

}

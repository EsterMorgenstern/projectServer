using DAL.Models;

namespace BLL.Models
{
    public class BLLStudentHealthFund
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }
        public int TreatmentsUsed { get; set; }   
        public int ReportedTreatments { get; set; }
        public int? CommitmentTreatments { get; set; }
        public int? RegisteredTreatments { get; set; }
        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }

    }
    public class BLLStudentHealthFundPerfect
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int Age { get; set; }
        public string? City { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartDateGroup { get; set; }
        public string? GroupName { get; set; }
        public int TreatmentsUsed { get; set; }
        public int? CommitmentTreatments { get; set; }
        public int? RegisteredTreatments { get; set; }
        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }
        public int ReportedTreatments { get; set; }


    }
}

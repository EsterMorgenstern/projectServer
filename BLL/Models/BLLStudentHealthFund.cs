namespace BLL.Models
{
    public class BLLStudentHealthFund
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }
        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }
        public byte? StandingOrderDay { get; set; }
        public int StandingOrderHandledMonth { get; set; }
    }

    public class BLLHealthFundCommitment
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
        public bool IsActive { get; set; }
        public byte? StandingOrderDay { get; set; }
        public int StandingOrderHandledMonth { get; set; }
    }

    public class BLLStudentHealthFundPerfect
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? OfficialFirstName { get; set; }
        public int Age { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartDateGroup { get; set; }
        public string? GroupName { get; set; }

        public int TreatmentsUsed { get; set; }          // ממתין לדיווח
        public int ReportedTreatments { get; set; }      // דווחו
        public int CommitmentTreatments { get; set; }    // מספר התחייבויות
        public int RegisteredTreatments { get; set; }    // התחייבויות שנוצלו

        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }
        public byte? StandingOrderDay { get; set; }
        public int StandingOrderHandledMonth { get; set; }

        public List<BLLHealthFundCommitment>? Commitments { get; set; }
    }
}
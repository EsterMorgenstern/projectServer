namespace DAL.Models
{
    public class StudentHealthFund
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int HealthFundId { get; set; }
        public DateTime StartDate { get; set; }
        public int TreatmentsUsed { get; set; }  // טיפולים שלא דווחו
        public int ReportedTreatments { get; set; } // טיפולים שדווחו
        public int? CommitmentTreatments { get; set; } // טיפולים שיש עליהם התחייבות
        public int? RegisteredTreatments { get; set; }  // טיפולים שנרשם אליהם
        public string? ReferralFilePath { get; set; }
        public string? CommitmentFilePath { get; set; }
        public string? Notes { get; set; }

        public Student? Student { get; set; }
        public HealthFund? HealthFund { get; set; }

        // קשרים לטבלאות החדשות
        public ICollection<ReportedDate>? ReportedDates { get; set; }
        public ICollection<UnreportedDate>? UnreportedDates { get; set; }
    }

    

}

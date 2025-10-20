using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class ReportedDate
    {
        public int Id { get; set; }
        public int StudentHealthFundId { get; set; }

        [Column("DateReported")] 
        public DateTime DateReported { get; set; } // Renamed to avoid conflict with class name

        public StudentHealthFund? StudentHealthFund { get; set; }
    }
}

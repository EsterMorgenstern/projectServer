namespace DAL.Models
{

    public class UnreportedDate
    {
        public int Id { get; set; }
        public int StudentHealthFundId { get; set; }
        public DateTime DateUnreported { get; set; }

        public StudentHealthFund? StudentHealthFund { get; set; }
    }
}

namespace BLL.Models
{
    public class BLLStudent
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string City { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public string HealthFund { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public DateTime LastActivityDate { get; set; }
    }
}

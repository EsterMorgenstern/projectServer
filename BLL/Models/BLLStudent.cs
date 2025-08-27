namespace BLL.Models
{
    public class BLLStudent
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string SecondaryPhone { get; set; } = string.Empty;
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public string HealthFund { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public DateTime LastActivityDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

    }
}

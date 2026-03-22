namespace BLL.Models
{
    public class BLLStudentWithNotesDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string SecondaryPhone { get; set; } = "";
        public int Age { get; set; }
        public string City { get; set; } = "";
        public string School { get; set; } = "";
        public string Class { get; set; } = "";
        public string Sector { get; set; } = "";
        public DateTime LastActivityDate { get; set; }
        public string Status { get; set; } = "";
        public string Email { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public string IdentityCard { get; set; } = "";
        public int HealthFundId { get; set; }
        public string HealthFundName { get; set; } = "";
        public string HealthFundPlan { get; set; } = "";
        public List<BLLStudentNote> Notes { get; set; } = new();
    }
}
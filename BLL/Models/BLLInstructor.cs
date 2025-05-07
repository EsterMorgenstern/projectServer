namespace BLL.Models
{
    public class BLLInstructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;   
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Sector { get; set; }

    }
}

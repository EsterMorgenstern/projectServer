﻿namespace BLL.Models
{
    public class BLLUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }
        public string? Role { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class BLLStudentNote
    {
        public int NoteId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public string? AuthorName { get; set; }

        [Required]
        public string? AuthorRole { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 5)]
        public string? NoteContent { get; set; }

        [Required]
        public string NoteType { get; set; } = string.Empty;

        public string? Priority { get; set; }

        public bool? IsPrivate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool? IsActive { get; set; }


    }

}
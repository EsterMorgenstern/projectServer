using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentNote
{
    public int NoteId { get; set; }

    public int StudentId { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public string AuthorRole { get; set; } = null!;

    public string NoteContent { get; set; } = null!;

    public string NoteType { get; set; } = null!;

    public string? Priority { get; set; }

    public bool? IsPrivate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Student Student { get; set; } = null!;
}

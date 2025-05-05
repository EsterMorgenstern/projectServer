using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public partial class Course
{
    [Key]
    public int CourseId { get; set; }

    [StringLength(20)]
    public string CouresName { get; set; } = null!;

    public int InstructorId { get; set; }

    public int? NumOfStudents { get; set; }

    public int? MaxNumOfStudents { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }
    public string? Branch { get; set; }
    public string? City { get; set; }
    public string? Group { get; set; }

    [ForeignKey("InstructorId")]
    [InverseProperty("Courses")]
    public virtual Instructor Instructor { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}

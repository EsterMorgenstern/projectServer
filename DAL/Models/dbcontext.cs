using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class Dbcontext : DbContext
{
    public Dbcontext()
    {
    }

    public Dbcontext(DbContextOptions<Dbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupStudent> GroupStudents { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Esty\\server\\Courses.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Hebrew_CI_AS");

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69261C4C356667");

            entity.ToTable("Attendance");

            entity.HasOne(d => d.Group).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Attendanc__Group__0B91BA14");

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Attendanc__Stude__0C85DE4D");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FC5E7E311AC");

            entity.Property(e => e.Address).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__tmp_ms_x__C92D71A7ECD46B42");

            entity.Property(e => e.CouresName).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36AE8EC77DE");

            entity.Property(e => e.AgeRange).HasMaxLength(20);
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.GroupName).HasMaxLength(20);
            entity.Property(e => e.Sector).HasMaxLength(20);

            entity.HasOne(d => d.Branch).WithMany(p => p.Groups)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__BranchId__02FC7413");

            entity.HasOne(d => d.Course).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__CourseId__03F0984C");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Groups)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__Instruct__04E4BC85");
        });

        modelBuilder.Entity<GroupStudent>(entity =>
        {
            entity.HasKey(e => e.GroupStudentId).HasName("PK__GroupStu__079A8A0E49A49032");

            entity.Property(e => e.EnrollmentDate).HasColumnName("EnrollmentDate ");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupStud__Group__08B54D69");

            entity.HasOne(d => d.Student).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupStud__Stude__07C12930");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC079FB7DAB8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Sector).HasMaxLength(20);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07CC8B0965");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.HealthFund).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.School).HasMaxLength(20);
            entity.Property(e => e.Sector).HasMaxLength(50);
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK__StudentC__5E57FC83E99183C9");

            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCo__Cours__5DCAEF64");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCo__Stude__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

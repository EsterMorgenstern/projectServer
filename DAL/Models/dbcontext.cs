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

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Esty\\server\\Courses.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Hebrew_CI_AS");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__tmp_ms_x__C92D71A7ECD46B42");

            entity.Property(e => e.CouresName).IsFixedLength();

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__Instruc__5CD6CB2B");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC079FB7DAB8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).IsFixedLength();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC07CC8B0965");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.CourseId }).HasName("PK__StudentC__5E57FC83E99183C9");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCo__Cours__5DCAEF64");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentCo__Stude__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

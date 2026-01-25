using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL.Models;

public partial class dbcontext : DbContext
{
    public dbcontext()
    {
    }

    public dbcontext(DbContextOptions<dbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }
    public virtual DbSet<Branch> Branches { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupStudent> GroupStudents { get; set; }
    public virtual DbSet<Instructor> Instructors { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<StudentNote> StudentNotes { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<LessonCancellations> LessonCancellations { get; set; }
    public virtual DbSet<HealthFund> HealthFunds { get; set; }
    public virtual DbSet<StudentHealthFund> StudentHealthFunds { get; set; }
    public virtual DbSet<ReportedDate> ReportedDates { get; set; }
    public virtual DbSet<UnreportedDate> UnreportedDates { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69261CFE17C02C");
            entity.ToTable("Attendance");
            entity.HasOne(d => d.Group).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Attendanc__Group__46E78A0C");
            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Attendanc__Stude__47DBAE45");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FC52ADF22B3");
            entity.Property(e => e.Address).HasMaxLength(20);
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.HasOne(d => d.Course).WithMany(p => p.Branches)
               .HasForeignKey(d => d.CourseId)
               .OnDelete(DeleteBehavior.ClientSetNull)
              ;

        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A76A479A52");
            entity.Property(e => e.CouresName).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(50);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36AF8EA9CDA");
            entity.Property(e => e.AgeRange).HasMaxLength(20);
            entity.Property(e => e.DayOfWeek).HasMaxLength(20);
            entity.Property(e => e.GroupName);
            entity.Property(e => e.IsActive);
            entity.Property(e => e.Sector).HasMaxLength(20);
            entity.HasOne(d => d.Branch).WithMany(p => p.Groups)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__BranchId__3E52440B");
            entity.HasOne(d => d.Course).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__CourseId__3F466844");
            entity.HasOne(d => d.Instructor).WithMany(p => p.Groups)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__Instruct__403A8C7D");
        });

        modelBuilder.Entity<GroupStudent>(entity =>
        {
            entity.HasKey(e => e.GroupStudentId).HasName("PK__GroupStu__079A8A0E02514AB6");
            entity.Property(e => e.EnrollmentDate).HasColumnName("EnrollmentDate ");
            entity.HasOne(d => d.Group).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupStud__Group__440B1D61");
            entity.HasOne(d => d.Student).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupStud__Stude__4316F928");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC07D549CD92");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Sector).HasMaxLength(20);
        });

        // עדכון טבלת Payment עם הקשרים החדשים
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38F825FE64");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Notes).HasMaxLength(200);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            // שדות חדשים
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("PENDING");
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // קשרים קיימים
            entity.HasOne(d => d.Student).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Payments__Studen__4AB81AF0");

            // קשר חדש ל-PaymentMethod
            entity.HasOne(d => d.PaymentMethodDetails).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Payments__PaymentMethod");

            // קשר חדש ל-Group
            entity.HasOne(d => d.Group).WithMany(p => p.Payments)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Payments__Group");
        });

        // הוספת טבלת PaymentMethods
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1F3A1B2C3D4");
            entity.ToTable("PaymentMethods");

            entity.Property(e => e.MethodType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.LastFourDigits)
                .HasMaxLength(4);

            entity.Property(e => e.CardType)
                .HasMaxLength(50);

            entity.Property(e => e.BankName)
                .HasMaxLength(100);

            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(100);

            // קשר ל-Student
            entity.HasOne(d => d.Student).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__PaymentMethods__Student");

            // אינדקס ייחודי - רק אמצעי תשלום אחד יכול להיות ברירת מחדל לכל תלמיד
            entity.HasIndex(e => new { e.StudentId, e.IsDefault })
                .HasFilter("IsDefault = 1")
                .IsUnique()
                .HasDatabaseName("UQ_Student_DefaultPaymentMethod");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC0799B46BC2");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.Class).HasMaxLength(10);
            entity.Property(e => e.LastName);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.SecondaryPhone).HasMaxLength(20);
            entity.Property(e => e.School).HasMaxLength(20);
            entity.Property(e => e.Sector).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.Email);
            entity.Property(e => e.CreatedBy);
            entity.Property(e => e.IdentityCard).HasMaxLength(20);
            entity.HasOne(s => s.HealthFundForStudent)
               .WithMany()
               .HasForeignKey(s => s.HealthFundId)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentNote>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__StudentN__EACE355FC9BE3B08");
            entity.Property(e => e.AuthorName).HasMaxLength(100);
            entity.Property(e => e.AuthorRole).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsPrivate).HasDefaultValue(false);
            entity.Property(e => e.NoteType).HasMaxLength(50);
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("Normal");
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Student).WithMany(p => p.StudentNotes)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentNotes_Student");
        });

        modelBuilder.Entity<LessonCancellations>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LessonCancellations");
            entity.ToTable("LessonCancellations");
            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("date");
            entity.Property(e => e.Reason)
                .IsRequired()
                .HasColumnType("nvarchar(max)");
            // שינוי המאפיין Created_at עם ממיר ערכים
            entity.Property(e => e.Created_at)
                .HasDefaultValueSql("GETDATE()")
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue), // המרה מ-DateOnly ל-DateTime
                    v => DateOnly.FromDateTime(v)        // המרה מ-DateTime ל-DateOnly
                )
                .HasColumnType("datetime");
            entity.Property(e => e.Created_by)
                .HasColumnType("nvarchar(max)");

            entity.HasIndex(e => new { e.GroupId, e.Date })
                .IsUnique()
                .HasDatabaseName("UQ_Group_Date");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07E1248B76");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
        });
        modelBuilder.Entity<HealthFund>(entity =>
        {
            entity.HasKey(e => e.HealthFundId);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FundType).HasMaxLength(100);
            entity.Property(e => e.PricePerLesson).HasColumnType("decimal(10,2)");
            entity.Property(e => e.MonthlyPrice).HasColumnType("decimal(10,2)");
            entity.Property(e => e.RequiresReferral).HasDefaultValue(false);
            entity.Property(e => e.RequiresCommitment).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<StudentHealthFund>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDate).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.TreatmentsUsed).HasDefaultValue(0);
            entity.Property(e => e.ReportedTreatments).HasDefaultValue(0);

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentHealthFunds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.HealthFund)
                .WithMany(p => p.StudentHealthFunds)
                .HasForeignKey(d => d.HealthFundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<ReportedDate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DateReported).IsRequired();
            entity.HasOne(d => d.StudentHealthFund)
                .WithMany(p => p.ReportedDates)
                .HasForeignKey(d => d.StudentHealthFundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<UnreportedDate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DateUnreported).IsRequired();
            entity.HasOne(d => d.StudentHealthFund)
                .WithMany(p => p.UnreportedDates)
                .HasForeignKey(d => d.StudentHealthFundId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId);
            entity.Property(e => e.LessonDate)
                .IsRequired()
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue), // המרה מ-DateOnly ל-DateTime
                    v => DateOnly.FromDateTime(v)
                )
                .HasColumnType("date");
            entity.Property(e => e.LessonHour)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null, // TimeOnly? -> TimeSpan?
                    v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null // TimeSpan? -> TimeOnly?
                )
                .HasColumnType("time");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("future");
            entity.Property(e => e.IsReported)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100);

            // קשר לקבוצה
            entity.HasOne(e => e.Group)
                .WithMany(g => g.Lessons)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // קשר למדריך (יכול להיות null)
            entity.HasOne(e => e.Instructor)
                .WithMany()
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

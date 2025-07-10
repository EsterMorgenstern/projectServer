using BLL.Api;
using BLL;
using BLL.Services;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Api;
using DAL.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DALCourseService>();
builder.Services.AddScoped<DALGroupService>();
builder.Services.AddScoped<DALBranchService>();
builder.Services.AddScoped<DALStudentService>();
builder.Services.AddScoped<DALAttendanceService>();
builder.Services.AddScoped<DALGroupStudentService>();
builder.Services.AddScoped<DALInstructorService>();
builder.Services.AddScoped<DALStudentNoteService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBLL, BLLManager>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// בדיקת חיבור למסד נתונים
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<dbcontext>();
    await context.Database.CanConnectAsync();
    Console.WriteLine("✅ Database connection successful");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Database connection failed: {ex.Message}");
}

app.Run();

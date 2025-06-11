using BLL.Api;
using BLL;
using BLL.Services; // הוסף את זה
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Api;
using DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\project\\server\\CoursesDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"));

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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "My API",
        Description = "An example API for demonstration purposes"
    });
});

builder.Services.AddScoped<IBLL, BLLManager>();
builder.Services.AddCors(c => c.AddPolicy("AllowAll",
    option => option.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dalService = services.GetRequiredService<IDALStudentNote>();
        var bllService = services.GetRequiredService<IBLLStudentNote>();
        Console.WriteLine("✅ Services registered successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Service registration failed: {ex.Message}");
    }
}

app.Run();
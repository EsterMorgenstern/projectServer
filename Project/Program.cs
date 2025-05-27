using BLL.Api;
using BLL;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
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
//להעתיק אחרי הגדרת ה app
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

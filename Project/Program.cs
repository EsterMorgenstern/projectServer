using BLL.Api;
using BLL;
using BLL.Services;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Api;
using DAL.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// קריאת Connection String מ-appsettings.json
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

// עדכון CORS - הוספת הדומיינים הספציפיים 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",                    // React לפיתוח מקומי
            "http://localhost:5248",                    // API לפיתוח מקומי
            "https://coursenet.nethost.co.il",          // האתר הראשי 
            "https://api.coursenet.nethost.co.il"       // ה-API (למקרה של בקשות פנימיות)
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials(); // חשוב לאימות ועוגיות
    });
});

var app = builder.Build();

// שימוש ב-CORS המעודכן
app.UseCors("AllowSpecificOrigins");

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

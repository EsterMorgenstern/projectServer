﻿using BLL;
using BLL.Api;
using BLL.Services;
using DAL;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DAL services
builder.Services.AddScoped<IDAL, DALManager>();
builder.Services.AddScoped<IDALAttendance, DALAttendanceService>();
builder.Services.AddScoped<IDALGroup, DALGroupService>();

// Register BLL services
builder.Services.AddScoped<IBLLAttendance, BLLAttendanceService>();
builder.Services.AddScoped<IBLL, BLLManager>();

// Register hosted services
builder.Services.AddHostedService<DailyAttendanceMarker>();

// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

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

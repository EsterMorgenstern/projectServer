using BLL.Api;
using BLL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBLL,BLLManager>();

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

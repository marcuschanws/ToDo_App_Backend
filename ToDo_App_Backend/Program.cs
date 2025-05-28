using ToDo_App_Backend.Context;
using Microsoft.EntityFrameworkCore;
using ToDo_App_Backend.Services;
using ToDo_App_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configure database
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.ConfigureSqlite(connectionString);
builder.Services.ConfigureServices();

//builder.Services.AddDbContext<ToDoDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()));

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend",
      builder => builder.WithOrigins("http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

builder.WebHost.UseUrls("https://localhost:7121");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowFrontend");

try
{
  // Database initialisation
  using (var scope = app.Services.CreateScope())
  {
    var db = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    await db.Database.EnsureCreatedAsync();
  }
}
catch (Exception ex)
{
}

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

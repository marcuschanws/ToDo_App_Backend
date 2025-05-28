using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDo_App_Backend.Context;

namespace ToDo_App_Backend.Services
{
  public static class ServiceExtensions
  {
    private const string _logFolderPath = "log";

    public static void ConfigureSqlite(this IServiceCollection services, string? connectionString)
    {
      services.AddDbContext<ToDoDbContext>(options =>
          options.UseSqlite(connectionString));
    }
    public static void ConfigureServices(this IServiceCollection services)
    {
      services.AddScoped<IToDoService, ToDoService>();
    }
  }
}

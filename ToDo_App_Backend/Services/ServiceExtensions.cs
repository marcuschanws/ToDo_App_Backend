using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDo_App_Backend.Context;
using ToDo_App_Backend.Logger;

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

    //public static void InitLogger(this IServiceCollection services)
    //{
    //  services.AddHttpLogging(options =>
    //  {
    //    options.LoggingFields = HttpLoggingFields.All;
    //  });

    //  services.AddLogging();
    //}

    public static Logger.IAppLogger InitLogger(Dictionary<string, string> arguments)
    {
      string logPathDir = GetValidDirPath(arguments, _logFolderPath, "Log folder path");
      Console.WriteLine($"Log folder: {Path.GetFullPath(logPathDir)}");
      Logger.IAppLogger logger = new AppLogger(logPathDir, true);

      return logger;
    }

    private static string GetValidDirPath(Dictionary<string, string> arguments, string key, string name)
    {
      string? resultPath;

      if (arguments.TryGetValue(key, out string? path))
        resultPath = EnsureValidPath(path, name);
      else
      {
        string? logPath = "log";
        resultPath = EnsureValidPath(logPath, name);
      }

      return resultPath;
    }

    private static string EnsureValidPath(string? path, string name)
    {
      string resultPath = path;

      while (true)
      {
        if (!IsDirPathValid(resultPath))
        {
          // Ask for user input via console app if invalid file paths were entered in both cmd line arguments and App.config
          Console.WriteLine($"Check if {name} contains invalid characters or surpasses the path character limit. Please re-enter the path:");
          resultPath = Console.ReadLine();
          if (!IsDirPathValid(resultPath))
            continue;
        }

        break; // Valid paths, exit the loop
      }

      return resultPath;
    }

    private static bool IsDirPathValid(string? path)
    {
      char[] invalidPathChars = Path.GetInvalidPathChars();
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      string? fileName = Path.GetFileName(path);

      // Checking for invalid characters and length (or if app.config also contains a file path entry)
      if (string.IsNullOrEmpty(path) || path.Any(c => invalidPathChars.Contains(c)) || string.IsNullOrEmpty(fileName) ||
            fileName.Any(c => invalidFileNameChars.Contains(c)) || path.Length > 260)
        return false;

      // Check if absolute path can be returned
      try
      {
        Path.GetFullPath(path);
      }
      catch
      {
        return false;
      }

      return true;
    }

  }
}

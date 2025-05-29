using ToDo_App_Backend.Context;
using Microsoft.EntityFrameworkCore;
using ToDo_App_Backend.Services;
using Microsoft.AspNetCore.StaticFiles;
using System.Diagnostics;
using Microsoft.Extensions.FileProviders;
using ToDo_App.Logging;
using System.Runtime.InteropServices;

// Create start up log
var startupLogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoApp_Logs");
Directory.CreateDirectory(startupLogDirectory);
var bootstrapLogPath = Path.Combine(startupLogDirectory, $"Startup_Log-{DateTime.Now:ddMMyyyy}.txt");
var bootstrapLogger = new FileLogger(bootstrapLogPath);

try
{
  bootstrapLogger.LogInformation($"Application starting at {DateTime.Now}");

  // Get the current executable's directory
  var exePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)
              ?? AppDomain.CurrentDomain.BaseDirectory;

  var wwwrootPath = Path.Combine(exePath, "wwwroot");

  // Create wwwroot if it doesn't exist
  if (!Directory.Exists(wwwrootPath))
  {
    try
    {
      Directory.CreateDirectory(wwwrootPath);
      Console.WriteLine($"Created wwwroot directory at: {wwwrootPath}");
    }
    catch (Exception ex)
    {
      bootstrapLogger.LogError(ex, "Error creating wwwroot");
      Console.WriteLine($"Error creating wwwroot: {ex.Message}");
      return;
    }
  }

  var builder = WebApplication.CreateBuilder(new WebApplicationOptions
  {
    ContentRootPath = exePath,
    WebRootPath = wwwrootPath,
    Args = args,
  });

  // Add services to the container.
  // Configure logging
  var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoApp_Logs");
  if (!Directory.Exists(logDirectory))
  {
    Directory.CreateDirectory(logDirectory);
  }

  var logFilePath = Path.Combine(logDirectory, $"ToDoApp_Log-{DateTime.Now:ddMMyyyy}.txt");
  builder.Logging.AddSimpleConsole(options => options.SingleLine = true);
  builder.Logging.AddFile(logFilePath);

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
                         .AllowAnyHeader()
                         .AllowCredentials());
  });

  // Use Development
  builder.WebHost.UseUrls("https://localhost:7121");

  // Production
  //builder.WebHost.UseKestrel().ConfigureKestrel(options =>
  //{
  //  options.ListenAnyIP(7121);
  //});

  builder.Services.AddControllers();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  var app = builder.Build();

  // Add startup log
  var logger = app.Services.GetRequiredService<ILogger<Program>>();
  logger.LogInformation($"Application started at {DateTime.UtcNow}");

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
    bootstrapLogger.LogError(ex, "Error initialising database");
    Console.WriteLine("Error initialising database");
  }

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.UseStaticFiles(new StaticFileOptions
  {
    FileProvider = new PhysicalFileProvider(wwwrootPath),
    RequestPath = ""
  });

  app.MapFallbackToFile("index.html", new StaticFileOptions
  {
    FileProvider = new PhysicalFileProvider(wwwrootPath)
  });

  try
  {
    // Serve static files from wwwroot (React build)
    app.UseStaticFiles(new StaticFileOptions
    {
      ContentTypeProvider = new FileExtensionContentTypeProvider()
    });

    if (app.Environment.IsDevelopment())
    {
      app.Lifetime.ApplicationStarted.Register(() =>
      {
        var url = "http://localhost:3000";
        Process.Start(new ProcessStartInfo
        {
          FileName = url,
          UseShellExecute = true
        });
      });
    }
    else
    {
      // Automatically open browser when app starts
      app.Lifetime.ApplicationStarted.Register(() =>
      {
        var url = "http://localhost:7121";
        Process.Start(new ProcessStartInfo
        {
          FileName = url,
          UseShellExecute = true
        });
      });
    }
  }
  catch (Exception ex)
  {
    bootstrapLogger.LogError(ex, ex.Message);
    Console.WriteLine(ex.Message);
  }

  app.UseHttpsRedirection();

  app.UseAuthorization();

  app.MapControllers();

  app.Run();
}
catch (Exception ex)
{
  bootstrapLogger.LogCritical(ex, "Application startup failed");
  File.AppendAllText(bootstrapLogPath, $"\nFatal error: {ex}\n");

  Console.WriteLine($"Application failed to start: {ex.Message}");
  Console.WriteLine($"See logs in: {startupLogDirectory}");
  Console.WriteLine("Press any key to exit...");
  Console.ReadKey();
}
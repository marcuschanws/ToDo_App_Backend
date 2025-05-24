namespace ToDo_App_Backend.Models
{
  public class LoggingSettings
  {
    public string LogPath { get; set; } = Path.Combine("Logs", "app.log");
    public string LogFormat { get; set; } = "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}";
    public int RetentionDays { get; set; } = 7;
    public bool EnableConsoleLogging { get; set; } = true;
  }
}

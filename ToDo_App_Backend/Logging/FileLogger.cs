namespace ToDo_App.Logging
{
  public class FileLogger : ILogger
  {
    private readonly string _filePath;
    private readonly object _lock = new object();

    public FileLogger(string filePath)
    {
      _filePath = filePath;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
      if (formatter == null) return;

      lock (_lock)
      {
        var message = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{logLevel}] {formatter(state, exception)}";
        File.AppendAllText(_filePath, message + Environment.NewLine);
      }
    }
  }
}

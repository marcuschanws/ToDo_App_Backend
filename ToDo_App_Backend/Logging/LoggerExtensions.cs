namespace ToDo_App.Logging
{
  public static class LoggerExtensions
  {
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
      builder.AddProvider(new FileLoggerProvider(filePath));
      return builder;
    }
  }
}

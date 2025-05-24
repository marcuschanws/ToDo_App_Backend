namespace ToDo_App_Backend.Services
{
  public abstract class BaseService
  {
    private Logger.IAppLogger _logger;

    protected BaseService(Logger.IAppLogger logger)
    {
      _logger = logger;
    }

    protected void Log(string message)
    {
      _logger?.Log(string.Empty, message);
    }

    protected void LogError(Exception ex)
    {
      _logger?.Log("ERR", ex.Message, ex);
    }

    protected void Progress(string message)
    {
      Log(message);
    }
  }
}

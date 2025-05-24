namespace ToDo_App_Backend.Logger
{
  public interface IAppLogger
  {
    void Log(string category, string message, Exception ex = null);

    void Progress(string message);
  }
}

using ToDo_App_Backend.Models;

namespace ToDo_App_Backend.Services
{
  public interface IToDoService
  {
    Task<List<ToDoTask>> GetAllAsync();
    Task<ToDoTask?> GetByIdentAsync(Guid ident);
    Task<ToDoTask> CreateAsync(ToDoTask task);
    Task<bool> UpdateAsync(ToDoTask task);
    Task<bool> DeleteAsync(Guid ident);
    Task<ToDoTask> UpdateDeadlineAsync(ToDoTask task, DateTime? deadline);
    Task<ToDoTask> UpdateDescAsync(ToDoTask task, string description);
  }
}

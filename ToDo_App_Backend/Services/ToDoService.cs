using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ToDo_App_Backend.Context;
using ToDo_App_Backend.Models;

namespace ToDo_App_Backend.Services
{
  public class ToDoService: IToDoService
  {
    private ToDoDbContext _context;

    public ToDoService(ToDoDbContext context)
    {
      _context = context;
    }

    public async Task<List<ToDoTask>> GetAllAsync()
    {
      return await _context.Tasks.ToListAsync();
    }
    

    public async Task<ToDoTask?> GetByIdentAsync(Guid ident)
    {
      return await _context.Tasks.Where(i => i.Identifier == ident).FirstOrDefaultAsync();
    }

    public async Task<ToDoTask> CreateAsync(ToDoTask task)
    {
      task.IsDone = false;
      task.Identifier = Guid.NewGuid();
      CultureInfo.CurrentCulture.ClearCachedData();
      task.CreatedAt = DateTime.Now;

      if (task.Deadline != null && !DateTime.TryParse(task.Deadline.ToString(), CultureInfo.InvariantCulture, out var _))
        throw new InvalidOperationException($"Deadline specified is invalid: {task.Deadline.ToString()}");

      _context.Tasks.Add(task);
      await _context.SaveChangesAsync();
      return task;
    }

    public async Task<bool> UpdateAsync(ToDoTask task)
    {
      // Toggle if task state has been altered and save
      _context.Entry(task).State = EntityState.Modified;
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid ident)
    {
      var task = await GetByIdentAsync(ident);
      if (task == null) 
        return false;

      _context.Tasks.Remove(task);
      return await _context.SaveChangesAsync() > 0;
    }

    public Task<ToDoTask> UpdateDeadlineAsync(ToDoTask task, DateTime? deadline)
    {
      if (deadline == null)
        task.Deadline = deadline;
      else if (DateTime.TryParse(deadline.ToString(), CultureInfo.InvariantCulture, out var newDeadline))
        task.Deadline = newDeadline;
      else
        return Task.FromException(new InvalidOperationException($"Deadline provided is invalid: {deadline.ToString()}")) as Task<ToDoTask>;

      _context.SaveChangesAsync();
      return Task.FromResult(task);
    }

    public Task<ToDoTask> UpdateDescAsync(ToDoTask task, string description)
    {
      task.Description = description;
      _context.SaveChangesAsync();
      return Task.FromResult(task);
    }
  }
}

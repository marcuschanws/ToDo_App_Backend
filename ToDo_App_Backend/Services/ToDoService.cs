using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Sources;
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
      task.CreatedAt = DateTime.UtcNow;

      _context.Tasks.Add(task);
      await _context.SaveChangesAsync();
      return task;
    }

    public async Task<bool> UpdateAsync(ToDoTask task)
    {
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
  }
}

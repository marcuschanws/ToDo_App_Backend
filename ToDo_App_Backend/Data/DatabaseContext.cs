using Microsoft.EntityFrameworkCore;
using ToDo_App_Backend.Models;

namespace ToDo_App_Backend.Context
{
  public class ToDoDbContext: DbContext
  {
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) 
      : base(options) 
    { 
    }

    public DbSet<ToDoTask> Tasks => Set<ToDoTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ToDoTask>()
          .Property(t => t.Description)
          .HasMaxLength(500);
    }
  }
}

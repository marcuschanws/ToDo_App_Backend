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

      modelBuilder.Entity<ToDoTask>()
          .Property(t => t.IsPriority)
          .HasDefaultValue(false);

      modelBuilder.Entity<SchemaVersion>(entity =>
      {
        entity.ToTable("__SchemaVersions");
        entity.HasKey(v => v.Version);
      });
    }

    public class SchemaVersion
    {
      public string Version { get; set; }
      public DateTime Applied { get; set; }
    }
  }
}

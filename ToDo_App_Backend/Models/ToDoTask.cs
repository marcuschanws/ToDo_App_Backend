using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ToDo_App.Utility;

namespace ToDo_App_Backend.Models
{
  public class ToDoTask
  {
    public int Id { get; set; }
    
    /// <summary>
    /// Use Identifier for task item identification
    /// </summary>
    public Guid Identifier { get; set; }
    
    [Required]
    [StringLength(500, MinimumLength = 11)]
    public string Description { get; set; } = string.Empty;

    [JsonConverter(typeof(MultiFormatDateTimeConverter))]
    public DateTime? Deadline { get; set; }
    public bool IsDone { get; set; } = false;
    public bool IsPriority { get; set; } = false;
    public DateTime CreatedAt { get; set; }
  }
}

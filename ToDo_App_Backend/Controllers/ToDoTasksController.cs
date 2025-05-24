using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo_App_Backend.Context;
using ToDo_App_Backend.Models;
using ToDo_App_Backend.Services;

namespace ToDo_App_Backend.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ToDoTasksController : ControllerBase
  {
    private readonly IToDoService _service;

    public ToDoTasksController(IToDoService service)
    {
      _service = service;
    }

    /// <summary>
    /// Get All To Do Tasks 
    /// </summary>
    /// <returns>Task list</returns>
    [HttpGet("GetAllTasks")]
    public async Task<IActionResult> GetAllTasks()
    {
      return Ok(await _service.GetAllAsync());
    }
        
    /// <summary>
    /// Get specified Task based on identifier Guid
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns>Finds specified task entity</returns>
    [HttpGet("GetTaskByIdent")]
    public async Task<IActionResult> GetTaskByUID(Guid identifier)
    {
      return Ok(await _service.GetByIdentAsync(identifier));
    }

    /// <summary>
    /// Create new task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    [HttpPost("CreateTask")]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTask task)
    {
      if (task.Description.Length <= 10)
        return BadRequest("Description must be at least 11 characters");
      else if (task.Description.Length > 500)
        return BadRequest("Description cannot be more than 500 characters");

      return Ok(await _service.CreateAsync(task));
    }

    /// <summary>
    /// Mark task as complete
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpPut("ToggleTaskDone")]
    public async Task<IActionResult> ToggleDone(Guid identifier)
    {
      var task = await _service.GetByIdentAsync(identifier);
      if (task == null) 
        return NotFound();

      task.IsDone = !task.IsDone;
      return await _service.UpdateAsync(task) ? NoContent() : BadRequest();
    }

    /// <summary>
    /// Delete task either that is complete or if user chooses to
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpDelete("DeleteTask")]
    public async Task<IActionResult> Delete(Guid identifier)
    {
      return await _service.DeleteAsync(identifier) ? NoContent() : NotFound();
    }
  }
}

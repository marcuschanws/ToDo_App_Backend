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
    /// <returns>To-Do Task list</returns>
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
    [HttpGet("GetTaskByIdent/{identifier}")]
    public async Task<IActionResult> GetTaskByUID(Guid identifier)
    {
      return Ok(await _service.GetByIdentAsync(identifier));
    }

    /// <summary>
    /// Create new task
    /// </summary>
    /// <param name="description"></param>
    /// <param name="deadline"></param>
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
    /// Mark task as complete/done
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpPut("ToggleTaskDone/{identifier}")]
    public async Task<IActionResult> ToggleDone(Guid identifier)
    {
      var task = await _service.GetByIdentAsync(identifier);
      if (task == null) 
        return NotFound();

      task.IsDone = !task.IsDone;
      return await _service.UpdateAsync(task) ? Ok(task) : BadRequest();
    }

    /// <summary>
    /// Delete a task that the user chooses to
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpDelete("DeleteTask/{identifier}")]
    public async Task<IActionResult> Delete(Guid identifier)
    {
      return await _service.DeleteAsync(identifier) ? Ok($"Succesful deletion of task with UID: {identifier}") : NotFound();
    }
  }
}

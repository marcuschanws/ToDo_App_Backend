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
      try
      {
        return Ok(await _service.GetAllAsync());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
        
    /// <summary>
    /// Get specified Task based on identifier Guid
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns>Finds specified task entity</returns>
    [HttpGet("GetTaskByIdent/{identifier}")]
    public async Task<IActionResult> GetTaskByUID(Guid identifier)
    {
      try
      {
        return Ok(await _service.GetByIdentAsync(identifier));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
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
      try
      {
        if (task.Description.Length <= 10)
          return BadRequest("Description must be at least 11 characters");
        else if (task.Description.Length > 500)
          return BadRequest("Description cannot be more than 500 characters");

        return Ok(await _service.CreateAsync(task));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Toggle task between complete/done or Pending
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpPut("ToggleTaskDone/{identifier}")]
    public async Task<IActionResult> ToggleDone(Guid identifier)
    {
      try
      {
        var task = await _service.GetByIdentAsync(identifier);
        if (task == null)
          return NotFound();

        task.IsDone = !task.IsDone;
        return await _service.UpdateAsync(task) ? Ok(task) : BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Toggle between priority task state and non-priority state
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpPut("TogglePriority/{identifier}")]
    public async Task<IActionResult> TogglePriority(Guid identifier)
    {
      try
      {
        var task = await _service.GetByIdentAsync(identifier);
        if (task == null)
          return NotFound();

        task.IsPriority = !task.IsPriority;
        return await _service.UpdateAsync(task) ? Ok(task) : BadRequest();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Delete a task that the user chooses to
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    [HttpDelete("DeleteTask/{identifier}")]
    public async Task<IActionResult> Delete(Guid identifier)
    {
      try
      {
        return await _service.DeleteAsync(identifier) ? Ok($"Succesful deletion of task with UID: {identifier}") : NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Update the deadline of the task
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="newDeadline"></param>
    /// <returns></returns>
    [HttpPut("UpdateDeadline/{identifier}")]
    public async Task<IActionResult> UpdateDeadline(Guid identifier, [FromBody] DateTime? newDeadline)
    {
      try
      {
        var updatedTask = new ToDoTask();
        var task = await _service.GetByIdentAsync(identifier);
        if (task == null)
          return NotFound();

        if (newDeadline != task.Deadline)
        {
          updatedTask = await _service.UpdateDeadlineAsync(task, newDeadline);

          if(updatedTask == null)
            return BadRequest($"Deadline specified is invalid: {newDeadline.ToString()}");
        }
        else
          return BadRequest($"New deadline {newDeadline} is the same as old deadline {task.Deadline} for task with UID: {task.Identifier}");

        return Ok(updatedTask);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Allows users to update the description of the task
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="newDescription"></param>
    /// <returns></returns>
    [HttpPut("UpdateDescription/{identifier}")]
    public async Task<IActionResult> UpdateDescription(Guid identifier, [FromBody] string newDescription)
    {
      try
      {
        var updatedTask = new ToDoTask();
        var task = await _service.GetByIdentAsync(identifier);
        if (task == null)
          return NotFound();

        if (newDescription != task.Description && newDescription.Length > 10 && newDescription.Length <= 500)
          updatedTask = await _service.UpdateDescAsync(task, newDescription);
        else if (newDescription == task.Description)
          return BadRequest($"New description is the same as the old description for task with UID: {task.Identifier}");
        else
          return BadRequest($"Ensure length of new description stays within 11 to 500 characters");

        return Ok(updatedTask);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}

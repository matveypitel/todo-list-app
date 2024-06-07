using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Abstractions;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolists/{todoListId:int}/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private static readonly Action<ILogger, string, string, Exception> LogError = LoggerMessage.Define<string, string>(
        LogLevel.Error,
        default,
        "Produce error response: [Error {Code}] {ExMessage}");

    private readonly ITaskItemDatabaseService databaseService;
    private readonly ILogger<TaskController> logger;
    private readonly IMapper mapper;

    public TaskController(ITaskItemDatabaseService databaseService, ILogger<TaskController> logger, IMapper mapper)
    {
        this.databaseService = databaseService;
        this.logger = logger;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

            newTaskItem.TodoListId = todoListId;
            newTaskItem.OwnerId = userId;
            newTaskItem.Assignee = userId;

            var taskItem = await this.databaseService.CreateAsync(newTaskItem);

            return this.CreatedAtAction(nameof(this.GetTaskItem), new { id = taskItem.Id, todoListId }, this.mapper.Map<TaskItemModel>(taskItem));
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemModel>>> GetTaskItems([FromRoute] int todoListId)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var taskItems = await this.databaseService.GetAllAsync(todoListId, userId);

            return this.Ok(this.mapper.Map<IEnumerable<TaskItemModel>>(taskItems));
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemModel>> GetTaskItem([FromRoute] int id, [FromRoute] int todoListId)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var taskItem = await this.databaseService.GetByIdAsync(id, todoListId, userId);

            return this.Ok(this.mapper.Map<TaskItemModel>(taskItem));
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTaskItem([FromRoute] int id, [FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var taskItem = this.mapper.Map<TaskItem>(taskItemModel);
            taskItem.OwnerId = userId;

            await this.databaseService.UpdateAsync(id, todoListId, taskItem);

            return this.NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTaskItem(int id, [FromRoute] int todoListId)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            await this.databaseService.DeleteAsync(id, todoListId, userId);

            return this.NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }
}

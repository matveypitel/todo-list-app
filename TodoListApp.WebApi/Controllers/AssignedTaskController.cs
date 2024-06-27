using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing assigned tasks.
/// </summary>
[Authorize]
[Route("api/tasks/assigned_to_me")]
[ApiController]
public class AssignedTaskController : ControllerBase
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly IMapper mapper;
    private readonly ITaskDatabaseService databaseService;
    private readonly ILogger<AssignedTaskController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignedTaskController"/> class.
    /// </summary>
    /// <param name="databaseService">The task database service.</param>
    /// <param name="mapper">The mapper.</param>
    public AssignedTaskController(ITaskDatabaseService databaseService, IMapper mapper, ILogger<AssignedTaskController> logger)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
        this.logger = logger;
    }

    /// <summary>
    /// GET: api/tasks/assigned_to_me.
    /// Gets the assigned tasks for the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="status">The task status.</param>
    /// <param name="sort">The sort order.</param>
    /// <returns>The paged list of assigned tasks.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> GetAssignedTasksToUser(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? sort = null)
    {
        var userName = this.GetUserName();

        var taskItems = await this.databaseService.GetPagedListOfAssignedTasksToUserAsync(userName, page, pageSize, status, sort);

        if (taskItems.TotalCount != 0 && page > (int)Math.Ceiling((double)taskItems.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"GetAssignedTasksToUser : {userName}", null);
        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(taskItems));
    }

    /// <summary>
    /// GET: api/tasks/assigned_to_me/{id}.
    /// Gets the task with the specified ID for the current user.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The task item.</returns>
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<TaskItemModel>> GetTaskById([FromRoute] int id)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetAssignedTaskByIdAsync(id, userName);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Got assigned task where id = {taskItem.Id}", null);
        return this.Ok(this.mapper.Map<TaskItemModel>(taskItem));
    }

    /// <summary>
    /// PUT: api/tasks/assigned_to_me/status/{id}.
    /// Updates the status of the task with the specified ID for the current user.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="taskItemModel">The updated task item.</param>
    /// <returns>The updated task item.</returns>
    [HttpPut]
    [Route("status/{id}")]
    public async Task<IActionResult> UpdateTaskStatus([FromRoute] int id, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var taskItem = this.mapper.Map<TaskItem>(taskItemModel);

        await this.databaseService.UpdateTaskStatusAsync(id, userName, taskItem);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Updated assigned task where id = {taskItem.Id}", null);
        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

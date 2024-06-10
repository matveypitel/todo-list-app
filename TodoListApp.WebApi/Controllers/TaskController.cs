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
    private readonly ITaskDatabaseService databaseService;
    private readonly IMapper mapper;

    public TaskController(ITaskDatabaseService databaseService, IMapper mapper)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var userName = this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

        newTaskItem.TodoListId = todoListId;
        newTaskItem.OwnerId = userId;
        newTaskItem.Assignee = userName;

        var taskItem = await this.databaseService.CreateTaskAsync(newTaskItem);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId },
            this.mapper.Map<TaskItemModel>(taskItem));
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> GetTaskItems(
        [FromRoute] int todoListId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var taskItems = await this.databaseService.GetListOfTasksAsync(todoListId, userId, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(taskItems));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemModel>> GetTaskDetails([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userId);

        return this.Ok(this.mapper.Map<TaskItemModel>(taskItem));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTaskItem([FromRoute] int id, [FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var taskItem = this.mapper.Map<TaskItem>(taskItemModel);
        taskItem.OwnerId = userId;

        await this.databaseService.UpdateTaskAsync(id, todoListId, taskItem);

        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTaskItem([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        await this.databaseService.DeleteTaskAsync(id, todoListId, userId);

        return this.NoContent();
    }
}

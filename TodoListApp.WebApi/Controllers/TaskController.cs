using System.Globalization;
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
    private readonly ITaskItemDatabaseService databaseService;
    private readonly IMapper mapper;

    public TaskController(ITaskItemDatabaseService databaseService, IMapper mapper)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

        newTaskItem.TodoListId = todoListId;
        newTaskItem.OwnerId = userId;
        newTaskItem.Assignee = userId;

        var taskItem = await this.databaseService.CreateTaskAsync(newTaskItem);

        return this.CreatedAtAction(nameof(this.GetTaskDetails), new { id = taskItem.Id, todoListId }, this.mapper.Map<TaskItemModel>(taskItem));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemModel>>> GetTaskItems([FromRoute] int todoListId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var taskItems = await this.databaseService.GetListOfTasksAsync(todoListId, userId);

        return this.Ok(this.mapper.Map<IEnumerable<TaskItemModel>>(taskItems));
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
    public async Task<ActionResult> DeleteTaskItem(int id, [FromRoute] int todoListId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        await this.databaseService.DeleteTaskAsync(id, todoListId, userId);

        return this.NoContent();
    }
}

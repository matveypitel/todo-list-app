using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolists/{todoListId:int}/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskDatabaseService databaseService;
    private readonly ITagDatabaseService tagDatabaseService;
    private readonly IMapper mapper;

    public TaskController(ITaskDatabaseService databaseService, IMapper mapper, ITagDatabaseService tagDatabaseService)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
        this.tagDatabaseService = tagDatabaseService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

        newTaskItem.TodoListId = todoListId;
        newTaskItem.Owner = userName;
        newTaskItem.AssignedTo = userName;

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
        var userName = this.GetUserName();

        var taskItems = await this.databaseService.GetPagedListOfTasksAsync(todoListId, userName, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(taskItems));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemModel>> GetTaskDetails([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        return this.Ok(this.mapper.Map<TaskItemModel>(taskItem));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTaskItem([FromRoute] int id, [FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var taskItem = this.mapper.Map<TaskItem>(taskItemModel);
        taskItem.Owner = userName;

        await this.databaseService.UpdateTaskAsync(id, todoListId, taskItem);

        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTaskItem([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        await this.databaseService.DeleteTaskAsync(id, todoListId, userName);

        return this.NoContent();
    }

    [HttpPost]
    [Route("{id}/tags")]
    public async Task<ActionResult<TagModel>> AddTagToTaskItem([FromRoute] int todoListId, [FromRoute] int id, [FromBody] TagModel tagModel)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        var tag = this.mapper.Map<Tag>(tagModel);

        var newTag = await this.tagDatabaseService.AddTagToTaskAsync(id, tag);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId, },
            this.mapper.Map<TagModel>(newTag));
    }

    [HttpPut]
    [Route("{id}/tags/{tagId}")]
    public async Task<ActionResult> UpdateTagOfTaskItem(
        [FromRoute] int todoListId,
        [FromRoute] int id,
        [FromRoute] int tagId,
        [FromBody] TagModel tag)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        await this.tagDatabaseService.UpdateTagAsync(tagId, id, this.mapper.Map<Tag>(tag));

        return this.NoContent();
    }

    [HttpDelete]
    [Route("{id}/tags/{tagId}")]
    public async Task<ActionResult> DeleteTagOfTaskItem([FromRoute] int todoListId, [FromRoute] int id, [FromRoute] int tagId)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        await this.tagDatabaseService.DeleteTagAsync(tagId, id);

        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

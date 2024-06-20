using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolists/{todoListId}/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskDatabaseService databaseService;
    private readonly ITagDatabaseService tagDatabaseService;
    private readonly ICommentDatabaseService commentDatabaseService;
    private readonly IMapper mapper;

    public TaskController(ITaskDatabaseService databaseService, IMapper mapper, ITagDatabaseService tagDatabaseService, ICommentDatabaseService commentDatabaseService)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
        this.tagDatabaseService = tagDatabaseService;
        this.commentDatabaseService = commentDatabaseService;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

        newTaskItem.TodoListId = todoListId;
        newTaskItem.Owner = userName;
        newTaskItem.AssignedTo = userName;

        var taskItem = await this.databaseService.CreateTaskAsync(newTaskItem, userName);

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

        if (taskItems.TotalCount != 0 && page > (int)Math.Ceiling((double)taskItems.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

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

        await this.databaseService.UpdateTaskAsync(id, todoListId, taskItem, userName);

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

        var newTag = await this.tagDatabaseService.AddTagToTaskAsync(id, tag, userName);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId, },
            this.mapper.Map<TagModel>(newTag));
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

        await this.tagDatabaseService.DeleteTagAsync(tagId, id, userName);

        return this.NoContent();
    }

    [HttpGet]
    [Route("role")]
    public async Task<ActionResult<TodoListRole>> GetUserRoleInTodoList([FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        var role = await this.databaseService.GetUserRoleInTodoListAsync(todoListId, userName);

        return this.Ok(role);
    }

    [HttpGet]
    [Route("{id}/tags/{tagId}")]
    public async Task<ActionResult<TagModel>> GetTagById([FromRoute] int id, [FromRoute] int tagId)
    {
        var userName = this.GetUserName();

        var tag = await this.tagDatabaseService.GetTagByIdAsync(tagId, id, userName);

        if (tag == null)
        {
            return this.NotFound();
        }

        return this.Ok(this.mapper.Map<TagModel>(tag));
    }

    [HttpGet]
    [Route("{id}/comments")]
    public async Task<ActionResult<PagedModel<CommentModel>>> GetCommentsOfTaskItem(
        [FromRoute] int id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var comments = await this.commentDatabaseService.GetPagedListOfCommentsAsync(id, page, pageSize);

        if (comments.TotalCount != 0 && page > (int)Math.Ceiling((double)comments.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<CommentModel>>(comments));
    }

    [HttpGet]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult<CommentModel>> GetCommentOfTaskItem([FromRoute] int id, [FromRoute] int commentId)
    {
        var comment = await this.commentDatabaseService.GetCommentByIdAsync(commentId, id);

        return this.Ok(this.mapper.Map<CommentModel>(comment));
    }

    [HttpPost]
    [Route("{id}/comments")]
    public async Task<ActionResult<CommentModel>> AddCommentToTaskItem([FromRoute] int id, [FromBody] CommentModel commentModel)
    {
        var userName = this.GetUserName();

        var comment = this.mapper.Map<Comment>(commentModel);
        comment.Owner = userName;

        var newComment = await this.commentDatabaseService.AddCommentToTaskAsync(comment, userName);

        return this.CreatedAtAction(
            nameof(this.GetCommentOfTaskItem),
            new { id, commentId = newComment.Id },
            this.mapper.Map<CommentModel>(newComment));
    }

    [HttpPut]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult> UpdateCommentOfTaskItem([FromRoute] int id, [FromRoute] int commentId, [FromBody] CommentModel commentModel)
    {
        var userName = this.GetUserName();

        var comment = this.mapper.Map<Comment>(commentModel);

        await this.commentDatabaseService.UpdateCommentAsync(commentId, id, comment, userName);

        return this.NoContent();
    }

    [HttpDelete]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult> DeleteCommentOfTaskItem([FromRoute] int id, [FromRoute] int commentId)
    {
        var userName = this.GetUserName();

        await this.commentDatabaseService.DeleteCommentAsync(commentId, id, userName);

        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

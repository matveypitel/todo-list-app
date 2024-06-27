using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.Models.Enums;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing tasks in a to-do list.
/// </summary>
[Authorize]
[Route("api/todolists/{todoListId}/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly ITaskDatabaseService databaseService;
    private readonly ITagDatabaseService tagDatabaseService;
    private readonly ICommentDatabaseService commentDatabaseService;
    private readonly IMapper mapper;
    private readonly ILogger<TaskController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskController"/> class.
    /// </summary>
    /// <param name="databaseService">The task database service.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="tagDatabaseService">The tag database service.</param>
    /// <param name="commentDatabaseService">The comment database service.</param>
    public TaskController(
        ITaskDatabaseService databaseService,
        IMapper mapper,
        ITagDatabaseService tagDatabaseService,
        ICommentDatabaseService commentDatabaseService,
        ILogger<TaskController> logger)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
        this.tagDatabaseService = tagDatabaseService;
        this.commentDatabaseService = commentDatabaseService;
        this.logger = logger;
    }

    /// <summary>
    /// POST: api/todolists/{todoListId}/tasks.
    /// Creates a new task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskItemModel">The task item model.</param>
    /// <returns>The created task item.</returns>
    [HttpPost]
    public async Task<ActionResult<TaskItemModel>> CreateTaskItem([FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var newTaskItem = this.mapper.Map<TaskItem>(taskItemModel);

        newTaskItem.TodoListId = todoListId;
        newTaskItem.Owner = userName;
        newTaskItem.AssignedTo = userName;

        var taskItem = await this.databaseService.CreateTaskAsync(newTaskItem, userName);

        LogInformation(
            this.logger,
            DateTime.Now.ToString(CultureInfo.InvariantCulture),
            $"Task with id {newTaskItem.Id} updated in to-do list with id {todoListId}",
            null);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId },
            this.mapper.Map<TaskItemModel>(taskItem));
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks.
    /// Gets a paged list of task items.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of task items.</returns>
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

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"{taskItems.TotalCount} tasks found in to-do list with id {todoListId}",
           null);

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(taskItems));
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks/{id}.
    /// Gets the details of a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The task item details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemModel>> GetTaskDetails([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Task details with id {taskItem.Id} in to-do list with id {todoListId}",
           null);

        return this.Ok(this.mapper.Map<TaskItemModel>(taskItem));
    }

    /// <summary>
    /// PUT: api/todolists/{todoListId}/tasks/{id}.
    /// Updates a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="taskItemModel">The updated task item model.</param>
    /// <returns>The updated task item.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTaskItem([FromRoute] int id, [FromRoute] int todoListId, [FromBody] TaskItemModel taskItemModel)
    {
        var userName = this.GetUserName();

        var taskItem = this.mapper.Map<TaskItem>(taskItemModel);
        taskItem.Owner = userName;

        await this.databaseService.UpdateTaskAsync(id, todoListId, taskItem, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Task with id {taskItem.Id} updated in to-do list with id {todoListId}",
           null);

        return this.NoContent();
    }

    /// <summary>
    /// DELETE: api/todolists/{todoListId}/tasks/{id}.
    /// Deletes a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The result of the deletion.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTaskItem([FromRoute] int id, [FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        await this.databaseService.DeleteTaskAsync(id, todoListId, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Task with id {id} deleted in to-do list with id {todoListId}",
           null);

        return this.NoContent();
    }

    /// <summary>
    /// POST: api/todolists/{todoListId}/tasks/{id}/tags.
    /// Adds a tag to a task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="tagModel">The tag model.</param>
    /// <returns>The created tag.</returns>
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

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Tag with id {newTag.Id} added to task with id {id}",
           null);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId, },
            this.mapper.Map<TagModel>(newTag));
    }

    /// <summary>
    /// DELETE: api/todolists/{todoListId}/tasks/{id}/tags/{tagId}.
    /// Deletes a tag of a task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The result of the deletion.</returns>
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

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Tag with id {tagId} deleted from task with id {id}",
           null);

        return this.NoContent();
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks/role.
    /// Gets the user's role in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The user's role in the to-do list.</returns>
    [HttpGet]
    [Route("role")]
    public async Task<ActionResult<TodoListRole>> GetUserRoleInTodoList([FromRoute] int todoListId)
    {
        var userName = this.GetUserName();

        var role = await this.databaseService.GetUserRoleInTodoListAsync(todoListId, userName);

        return this.Ok(role);
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks/{id}/tags/{tagId}.
    /// Gets a tag by ID.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The tag details.</returns>
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

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Tag with id {tagId} found from task with id {id}",
           null);

        return this.Ok(this.mapper.Map<TagModel>(tag));
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks/{id}/comments.
    /// Gets a paged list of comments for a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of comments.</returns>
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

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"{comments.TotalCount} found in task with id {id}",
           null);

        return this.Ok(this.mapper.Map<PagedModel<CommentModel>>(comments));
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/tasks/{id}/comments/{commentId}.
    /// Gets a comment by ID.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>The comment details.</returns>
    [HttpGet]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult<CommentModel>> GetCommentOfTaskItem([FromRoute] int id, [FromRoute] int commentId)
    {
        var comment = await this.commentDatabaseService.GetCommentByIdAsync(commentId, id);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Comment with id {commentId} found from task with id {id}",
           null);

        return this.Ok(this.mapper.Map<CommentModel>(comment));
    }

    /// <summary>
    /// POST: api/todolists/{todoListId}/tasks/{id}/comments.
    /// Adds a comment to a task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="commentModel">The comment model.</param>
    /// <returns>The created comment.</returns>
    [HttpPost]
    [Route("{id}/comments")]
    public async Task<ActionResult<CommentModel>> AddCommentToTaskItem([FromRoute] int todoListId, [FromRoute] int id, [FromBody] CommentModel commentModel)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        var comment = this.mapper.Map<Comment>(commentModel);
        comment.Owner = userName;
        comment.TaskId = id;

        var newComment = await this.commentDatabaseService.AddCommentToTaskAsync(comment, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Comment with id {newComment.Id} added to task with id {id}",
           null);

        return this.CreatedAtAction(
            nameof(this.GetTaskDetails),
            new { id = taskItem.Id, todoListId = taskItem.TodoListId, },
            this.mapper.Map<CommentModel>(newComment));
    }

    /// <summary>
    /// PUT: api/todolists/{todoListId}/tasks/{id}/comments/{commentId}.
    /// Updates a comment of a task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <param name="commentModel">The updated comment model.</param>
    /// <returns>The result of the update.</returns>
    [HttpPut]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult> UpdateCommentOfTaskItem([FromRoute] int todoListId, [FromRoute] int id, [FromRoute] int commentId, [FromBody] CommentModel commentModel)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        var comment = this.mapper.Map<Comment>(commentModel);

        await this.commentDatabaseService.UpdateCommentAsync(commentId, id, comment, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Comment with id {commentId} updated from task with id {id}",
           null);

        return this.NoContent();
    }

    /// <summary>
    /// DELETE: api/todolists/{todoListId}/tasks/{id}/comments/{commentId}.
    /// Deletes a comment of a task item.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>The result of the deletion.</returns>
    [HttpDelete]
    [Route("{id}/comments/{commentId}")]
    public async Task<ActionResult> DeleteCommentOfTaskItem([FromRoute] int todoListId, [FromRoute] int id, [FromRoute] int commentId)
    {
        var userName = this.GetUserName();

        var taskItem = await this.databaseService.GetTaskByIdAsync(id, todoListId, userName);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        await this.commentDatabaseService.DeleteCommentAsync(commentId, id, userName);

        LogInformation(
           this.logger,
           DateTime.Now.ToString(CultureInfo.InvariantCulture),
           $"Comment with id {commentId} deleted from task with id {id}",
           null);

        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controls the operations related to tasks within to-do lists, including viewing, creating, editing, and deleting tasks.
/// </summary>
[Authorize]
[Route("todolists/{todoListId}/tasks")]
public class TaskController : Controller
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly ITaskWebApiService apiService;
    private readonly ITagWebApiService tagApiService;
    private readonly ICommentWebApiService commentApiService;
    private readonly ITodoListWebApiService todoListApiService;
    private readonly IMapper mapper;
    private readonly ILogger<TaskController> logger;
    private readonly int pageSize = 5;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskController"/> class with necessary services.
    /// </summary>
    /// <param name="apiService">The service for managing tasks via API.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="commentApiService">The service for managing comments via API.</param>
    /// <param name="userManager">The user manager for handling user-related operations.</param>
    /// <param name="tagApiService">The service for managing tags via API.</param>
    public TaskController(
        ITaskWebApiService apiService,
        IMapper mapper,
        ICommentWebApiService commentApiService,
        ITagWebApiService tagApiService,
        ITodoListWebApiService todoListApiService,
        ILogger<TaskController> logger)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.tagApiService = tagApiService;
        this.commentApiService = commentApiService;
        this.todoListApiService = todoListApiService;
        this.logger = logger;
    }

    /// <summary>
    /// Displays a paginated list of tasks for a specific to-do list.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="page">The page number to display.</param>
    /// <returns>A view with the list of tasks.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(int todoListId, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedTasksAsync(token, todoListId, page, this.pageSize);

        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;
        this.TempData["TodoListId"] = todoListId;
        this.TempData["CurrentPage"] = page;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view with {pagedResult.TotalCount} tasks", null);
        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }

    /// <summary>
    /// Displays the details of a specific task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="id">The identifier of the task to display.</param>
    /// <returns>A view with the task details.</returns>
    [HttpGet]
    [Route("{id}/details")]
    public async Task<IActionResult> Details(int todoListId, int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var task = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Gets details of task with id = {id}", null);
        return this.View(this.mapper.Map<TaskItemModel>(task));
    }

    /// <summary>
    /// Displays a form to create a new task within a specific to-do list.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <returns>A view containing the form for creating a new task.</returns>
    [HttpGet]
    [Route("create")]
    public IActionResult Create(int todoListId)
    {
        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;
        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of creation task", null);
        return this.View(new TaskItemModel() { TodoListId = todoListId });
    }

    /// <summary>
    /// Processes the creation of a new task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="model">The task model containing the data for the new task.</param>
    /// <returns>A redirection to the task list on success, or the same view with validation errors on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public async Task<IActionResult> Create(int todoListId, TaskItemModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newTask = this.mapper.Map<TaskItem>(model);
        newTask.TodoListId = todoListId;

        _ = await this.apiService.CreateTaskAsync(token, todoListId, newTask);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully create task with id = {newTask.Id}", null);
        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    /// <summary>
    /// Displays a form to edit an existing task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="id">The identifier of the task to edit.</param>
    /// <returns>A view containing the form for editing the task.</returns>
    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int todoListId, int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskToEdit = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        var todoList = await this.todoListApiService.GetTodoListByIdAsync(token, todoListId);
        var todoListUserNames = todoList.Users.Select(u => u.UserName).ToList();

        var usersList = todoListUserNames.Select(user => new SelectListItem(user, user))
            .ToList();

        this.ViewBag.Users = usersList;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of editing task with id = {id}", null);
        return this.View(this.mapper.Map<TaskItemModel>(taskToEdit));
    }

    /// <summary>
    /// Processes the update of an existing task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="id">The identifier of the task to update.</param>
    /// <param name="model">The updated task model.</param>
    /// <returns>A redirection to the task list on success, or the same view with validation errors on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int todoListId, int id, TaskItemModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var updatedTask = this.mapper.Map<TaskItem>(model);

        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        await this.apiService.UpdateTaskAsync(token, id, todoListId, updatedTask);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully edit task with id = {id}", null);
        return this.RedirectToAction(nameof(this.Index), new { todoListId, page = this.ViewBag.CurrentPage });
    }

    /// <summary>
    /// Deletes a specified task from a to-do list.
    /// </summary>
    /// <param name="id">The identifier of the task to delete.</param>
    /// /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <returns>A redirection to the task list.</returns>
    [HttpGet]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(int id, int todoListId)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskToDelete = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of deleting task with id = {id}", null);
        return this.View(this.mapper.Map<TaskItemModel>(taskToDelete));
    }

    /// <summary>
    /// Deletes a specified task from a to-do list after confirmation.
    /// </summary>
    /// <param name="id">The identifier of the task to delete.</param>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <returns>A redirection to the task list.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id, int todoListId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.DeleteTaskAsync(token, id, todoListId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully delete task with id = {id}", null);
        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    /// <summary>
    /// Displays a form to add a new tag to a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="taskId">The identifier of the task to add a tag to.</param>
    /// <returns>A view containing the form for adding a new tag.</returns>
    [HttpGet]
    [Route("{taskId}/tags/add")]
    public IActionResult AddTag(int todoListId, int taskId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view to add tag to task with id = {taskId}", null);
        return this.View(new TagModel());
    }

    /// <summary>
    /// Processes the addition of a new tag to a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="taskId">The identifier of the task to add a tag to.</param>
    /// <param name="model">The tag model containing the data for the new tag.</param>
    /// <returns>A redirection to the task details on success, or the same view with validation errors on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{taskId}/tags/add")]
    public async Task<IActionResult> AddTag(int todoListId, int taskId, TagModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newTag = this.mapper.Map<Tag>(model);

        _ = await this.tagApiService.AddTagToTaskAsync(token, todoListId, taskId, newTag);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully add tag - {newTag.Label} to task with id = {taskId}", null);
        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    /// <summary>
    /// Displays a form to delete a tag from a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="taskId">The identifier of the task containing the tag.</param>
    /// <param name="tagId">The identifier of the tag to delete.</param>
    /// <returns>A view containing the form for deleting the tag.</returns>
    [HttpGet]
    [Route("{taskId}/tags/delete/{tagId}")]
    public async Task<IActionResult> DeleteTag(int todoListId, int taskId, int tagId)
    {
        var token = TokenUtility.GetToken(this.Request);
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var tag = await this.tagApiService.GetTagByIdAsync(token, todoListId, taskId, tagId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view to delete tag from task with id = {taskId}", null);
        return this.View(this.mapper.Map<TagModel>(tag));
    }

    /// <summary>
    /// Processes the deletion of a tag from a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list.</param>
    /// <param name="taskId">The identifier of the task containing the tag.</param>
    /// <param name="tagId">The identifier of the tag to delete.</param>
    /// <returns>A redirection to the task details view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{taskId}/tags/deleteConfirmed")]
    public async Task<IActionResult> DeleteTagConfirmed(int todoListId, int taskId, int tagId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.tagApiService.DeleteTagAsync(token, todoListId, taskId, tagId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully delete tag with id = {tagId} from task with id = {taskId}", null);
        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    /// <summary>
    /// Displays the comments for a specific task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task containing the comments.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>A view displaying the paged list of comments.</returns>
    [HttpGet]
    [Route("{taskId}/comments")]
    public async Task<IActionResult> Comments(int todoListId, int taskId, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var pagedResult = await this.commentApiService.GetPagedListOfCommentsAsync(token, taskId, todoListId, page, this.pageSize);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of comments of task with id = {taskId}", null);
        return this.View(this.mapper.Map<PagedModel<CommentModel>>(pagedResult));
    }

    /// <summary>
    /// Displays a form to add a new comment to a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task to add a comment to.</param>
    /// <returns>The view to add a new comment.</returns>
    [HttpGet]
    [Route("{taskId}/comments/add")]
    public IActionResult AddComment(int todoListId, int taskId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of add comment to task with id = {taskId}", null);
        return this.View(new CommentModel());
    }

    /// <summary>
    /// Adds a new comment to a task.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task to add a comment to.</param>
    /// <param name="model">The comment model containing the comment details.</param>
    /// <returns>A redirection to the task details view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{taskId}/comments/add")]
    public async Task<IActionResult> AddComment(int todoListId, int taskId, CommentModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newComment = this.mapper.Map<Comment>(model);

        _ = await this.commentApiService.AddCommentToTaskAsync(token, todoListId, taskId, newComment);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully add comment with id = {newComment.Id} to task with id = {taskId}", null);
        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    /// <summary>
    /// Displays a form to edit an existing comment.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task containing the comment.</param>
    /// <param name="commentId">The identifier of the comment to edit.</param>
    /// <returns>The view to edit the comment.</returns>
    [HttpGet]
    [Route("{taskId}/comments/edit/{commentId}")]
    public async Task<IActionResult> EditComment(int todoListId, int taskId, int commentId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var commentToEdit = await this.commentApiService.GetCommentByIdAsync(token, commentId, todoListId, taskId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of editing comment in task with id = {taskId}", null);
        return this.View(this.mapper.Map<CommentModel>(commentToEdit));
    }

    /// <summary>
    /// Edits an existing comment.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="commentId">The identifier of the comment to edit.</param>
    /// <param name="taskId">The identifier of the task containing the comment.</param>
    /// <param name="model">The comment model containing the updated comment details.</param>
    /// <returns>A redirection to the comments list view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{taskId}/comments/edit/{commentId}")]
    public async Task<IActionResult> EditComment(int todoListId, int commentId, int taskId, CommentModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var updatedComment = this.mapper.Map<Comment>(model);

        await this.commentApiService.UpdateCommentAsync(token, commentId, todoListId, taskId, updatedComment);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully edit comment with id = {updatedComment.Id} in task with id = {taskId}", null);
        return this.RedirectToAction(nameof(this.Comments), new { todoListId, taskId });
    }

    /// <summary>
    /// Displays a form to delete an existing comment.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task containing the comment.</param>
    /// <param name="commentId">The identifier of the comment to delete.</param>
    /// <returns>The view to confirm the deletion of the comment.</returns>
    [HttpGet]
    [Route("{taskId}/comments/delete/{commentId}")]
    public async Task<IActionResult> DeleteComment(int todoListId, int taskId, int commentId)
    {
        var token = TokenUtility.GetToken(this.Request);
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var comment = await this.commentApiService.GetCommentByIdAsync(token, commentId, todoListId, taskId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning the view of deleting comment from task with id = {taskId}", null);
        return this.View(this.mapper.Map<CommentModel>(comment));
    }

    /// <summary>
    /// Deletes an existing comment after confirmation.
    /// </summary>
    /// <param name="todoListId">The identifier of the to-do list containing the task.</param>
    /// <param name="taskId">The identifier of the task containing the comment.</param>
    /// <param name="commentId">The identifier of the comment to delete.</param>
    /// <returns>A redirection to the comments list view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{taskId}/comments/deleteConfirmed")]
    public async Task<IActionResult> DeleteCommentConfirmed(int todoListId, int taskId, int commentId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.commentApiService.DeleteCommentAsync(token, commentId, todoListId, taskId);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully delete comment with id = {commentId} from task with id = {taskId}", null);
        return this.RedirectToAction(nameof(this.Comments), new { todoListId, taskId });
    }
}

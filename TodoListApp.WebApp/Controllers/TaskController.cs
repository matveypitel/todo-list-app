using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("todolists/{todoListId}/tasks")]
public class TaskController : Controller
{
    private readonly ITaskWebApiService apiService;
    private readonly ITagWebApiService tagApiService;
    private readonly ICommentWebApiService commentApiService;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public TaskController(ITaskWebApiService apiService, IMapper mapper, ICommentWebApiService commentApiService, UserManager<IdentityUser> userManager, ITagWebApiService tagApiService)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.userManager = userManager;
        this.tagApiService = tagApiService;
        this.commentApiService = commentApiService;
    }

    public int PageSize { get; set; } = 5;

    [HttpGet]
    public async Task<IActionResult> Index(int todoListId, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedTasksAsync(token, todoListId, page, this.PageSize);

        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;
        this.TempData["TodoListId"] = todoListId;

        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }

    [HttpGet]
    [Route("{id}/details")]
    public async Task<IActionResult> Details(int todoListId, int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var task = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;

        return this.View(this.mapper.Map<TaskItemModel>(task));
    }

    [HttpGet]
    [Route("create")]
    public IActionResult Create(int todoListId)
    {
        return this.View(new TaskItemModel() { TodoListId = todoListId });
    }

    [HttpPost]
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

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int todoListId, int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskToEdit = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        var users = this.userManager.Users.ToList();
        var usersList = users.Select(user => new SelectListItem(user.UserName, user.UserName))
            .ToList();
        this.ViewBag.Users = usersList;

        return this.View(this.mapper.Map<TaskItemModel>(taskToEdit));
    }

    [HttpPost]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int todoListId, int id, TaskItemModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var updatedTask = this.mapper.Map<TaskItem>(model);

        await this.apiService.UpdateTaskAsync(token, id, todoListId, updatedTask);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(int id, int todoListId)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskToDelete = await this.apiService.GetTaskByIdAsync(token, id, todoListId);

        return this.View(this.mapper.Map<TaskItemModel>(taskToDelete));
    }

    [HttpPost]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id, int todoListId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.DeleteTaskAsync(token, id, todoListId);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    [Route("{taskId}/tags/add")]
    public IActionResult AddTag(int todoListId, int taskId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        return this.View(new TagModel());
    }

    [HttpPost]
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

        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    [HttpGet]
    [Route("{taskId}/tags/delete/{tagId}")]
    public async Task<IActionResult> DeleteTag(int todoListId, int taskId, int tagId)
    {
        var token = TokenUtility.GetToken(this.Request);
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var tag = await this.tagApiService.GetTagByIdAsync(token, todoListId, taskId, tagId);

        return this.View(this.mapper.Map<TagModel>(tag));
    }

    [HttpPost]
    [Route("{taskId}/tags/deleteConfirmed")]
    public async Task<IActionResult> DeleteTagConfirmed(int todoListId, int taskId, int tagId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.tagApiService.DeleteTagAsync(token, todoListId, taskId, tagId);

        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    [HttpGet]
    [Route("{taskId}/comments")]
    public async Task<IActionResult> Comments(int todoListId, int taskId, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var userRole = await this.apiService.GetUserRoleInTodoListAsync(token, todoListId);

        this.TempData["UserRole"] = userRole;
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var pagedResult = await this.commentApiService.GetPagedListOfCommentsAsync(token, taskId, todoListId, page, this.PageSize);

        return this.View(this.mapper.Map<PagedModel<CommentModel>>(pagedResult));
    }

    [HttpGet]
    [Route("{taskId}/comments/add")]
    public IActionResult AddComment(int todoListId, int taskId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        return this.View(new CommentModel());
    }

    [HttpPost]
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

        return this.RedirectToAction(nameof(this.Details), new { todoListId, id = taskId });
    }

    [HttpGet]
    [Route("{taskId}/comments/edit/{commentId}")]
    public async Task<IActionResult> EditComment(int todoListId, int taskId, int commentId)
    {
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var commentToEdit = await this.commentApiService.GetCommentByIdAsync(token, commentId, todoListId, taskId);

        return this.View(this.mapper.Map<CommentModel>(commentToEdit));
    }

    [HttpPost]
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

        return this.RedirectToAction(nameof(this.Comments), new { todoListId, taskId });
    }

    [HttpGet]
    [Route("{taskId}/comments/delete/{commentId}")]
    public async Task<IActionResult> DeleteComment(int todoListId, int taskId, int commentId)
    {
        var token = TokenUtility.GetToken(this.Request);
        this.TempData["TaskId"] = taskId;
        this.TempData["TodoListId"] = todoListId;

        var comment = await this.commentApiService.GetCommentByIdAsync(token, commentId, todoListId, taskId);

        return this.View(this.mapper.Map<CommentModel>(comment));
    }

    [HttpPost]
    [Route("{taskId}/comments/deleteConfirmed")]
    public async Task<IActionResult> DeleteCommentConfirmed(int todoListId, int taskId, int commentId)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.commentApiService.DeleteCommentAsync(token, commentId, todoListId, taskId);

        return this.RedirectToAction(nameof(this.Comments), new { todoListId, taskId });
    }
}

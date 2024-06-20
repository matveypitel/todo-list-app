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
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public TaskController(ITaskWebApiService apiService, IMapper mapper, UserManager<IdentityUser> userManager, ITagWebApiService tagApiService)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.userManager = userManager;
        this.tagApiService = tagApiService;
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
}

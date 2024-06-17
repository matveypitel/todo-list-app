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
[Route("todolists/{todoListId}/share_users")]
public class ShareUserController : Controller
{
    private readonly IShareUserWebApiService apiService;
    private readonly ITodoListWebApiService todoListApiService;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;

    public ShareUserController(IShareUserWebApiService apiService, IMapper mapper, UserManager<IdentityUser> userManager, ITodoListWebApiService todoListApiService)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.userManager = userManager;
        this.todoListApiService = todoListApiService;
    }

    public int PageSize { get; set; } = 15;

    public async Task<IActionResult> Index(int todoListId, [FromQuery] int page = 1)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedListOfUsersInTodoListAsync(token, todoListId, page, this.PageSize);

        return this.View(this.mapper.Map<PagedModel<TodoListUserModel>>(pagedResult));
    }

    [HttpGet]
    [Route("add")]
    public async Task<IActionResult> Add(int todoListId)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var todoList = await this.todoListApiService.GetTodoListByIdAsync(token, todoListId);
        var todoListUserNames = todoList.Users.Select(u => u.UserName).ToList();

        var users = this.userManager.Users
        .Where(u => u.UserName != this.User.Identity!.Name && !todoListUserNames.Contains(u.UserName))
        .ToList();

        var usersList = users.Select(user => new SelectListItem(user.UserName, user.UserName))
            .ToList();
        this.ViewBag.Users = usersList;

        return this.View(new TodoListUserModel() { TodoListId = todoListId });
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add(int todoListId, TodoListUserModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newUser = this.mapper.Map<TodoListUser>(model);

        _ = await this.apiService.AddUserToTodoListAsync(token, todoListId, newUser);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    [Route("edit/{userName}")]
    public async Task<IActionResult> Edit(int todoListId, string userName)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var user = await this.apiService.GetUserInTodoListAsync(token, todoListId, userName);

        return this.View(this.mapper.Map<TodoListUserModel>(user));
    }

    [HttpPost]
    [Route("edit/{userName}")]
    public async Task<IActionResult> Edit(int todoListId, string userName, TodoListUserModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var user = this.mapper.Map<TodoListUser>(model);

        await this.apiService.UpdateUserRoleAsync(token, todoListId, userName, user);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }

    [HttpGet]
    [Route("delete/{userName}")]
    public async Task<IActionResult> Delete(int todoListId, string userName)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var todoUser = await this.apiService.GetUserInTodoListAsync(token, todoListId, userName);

        return this.View(this.mapper.Map<TodoListUserModel>(todoUser));
    }

    [HttpPost]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int todoListId, string userName)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.RemoveUserFromTodoListAsync(token, todoListId, userName);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }
}

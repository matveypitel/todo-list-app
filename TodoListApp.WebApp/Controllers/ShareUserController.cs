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

/// <summary>
/// Controller for managing shared users in a to-do list.
/// </summary>
[Authorize]
[Route("todolists/{todoListId}/share_users")]
public class ShareUserController : Controller
{
    private readonly IShareUserWebApiService apiService;
    private readonly ITodoListWebApiService todoListApiService;
    private readonly IMapper mapper;
    private readonly UserManager<IdentityUser> userManager;
    private readonly int pageSize = 15;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareUserController"/> class.
    /// </summary>
    /// <param name="apiService">The share user web API service.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="todoListApiService">The to-do list web API service.</param>
    public ShareUserController(IShareUserWebApiService apiService, IMapper mapper, UserManager<IdentityUser> userManager, ITodoListWebApiService todoListApiService)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.userManager = userManager;
        this.todoListApiService = todoListApiService;
    }

    /// <summary>
    /// Displays the list of shared users in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <returns>The view displaying the list of shared users.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(int todoListId, [FromQuery] int page = 1)
    {
        this.TempData["TodoListId"] = todoListId;
        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedListOfUsersInTodoListAsync(token, todoListId, page, this.pageSize);

        return this.View(this.mapper.Map<PagedModel<TodoListUserModel>>(pagedResult));
    }

    /// <summary>
    /// Displays the view for adding a new shared user to a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The view for adding a new shared user.</returns>
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

    /// <summary>
    /// Handles the HTTP POST request for adding a new shared user to a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="model">The model containing the details of the shared user.</param>
    /// <returns>The action result after adding the shared user.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
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

    /// <summary>
    /// Displays the view for editing a shared user in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <returns>The view for editing a shared user.</returns>
    [HttpGet]
    [Route("edit/{userName}")]
    public async Task<IActionResult> Edit(int todoListId, string userName)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var user = await this.apiService.GetUserInTodoListAsync(token, todoListId, userName);

        return this.View(this.mapper.Map<TodoListUserModel>(user));
    }

    /// <summary>
    /// Handles the HTTP POST request for editing a shared user in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <param name="model">The model containing the updated details of the shared user.</param>
    /// <returns>The action result after editing the shared user.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
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

    /// <summary>
    /// Displays the view for deleting a shared user from a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <returns>The view for deleting a shared user.</returns>
    [HttpGet]
    [Route("delete/{userName}")]
    public async Task<IActionResult> Delete(int todoListId, string userName)
    {
        this.TempData["TodoListId"] = todoListId;
        var token = TokenUtility.GetToken(this.Request);

        var todoUser = await this.apiService.GetUserInTodoListAsync(token, todoListId, userName);

        return this.View(this.mapper.Map<TodoListUserModel>(todoUser));
    }

    /// <summary>
    /// Handles the HTTP POST request for confirming the deletion of a shared user from a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <returns>The action result after confirming the deletion of the shared user.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int todoListId, string userName)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.RemoveUserFromTodoListAsync(token, todoListId, userName);

        return this.RedirectToAction(nameof(this.Index), new { todoListId });
    }
}

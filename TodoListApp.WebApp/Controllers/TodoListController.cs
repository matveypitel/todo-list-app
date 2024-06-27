using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing to-do lists.
/// </summary>
[Authorize]
[Route("todolists")]
public class TodoListController : Controller
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly ITodoListWebApiService apiService;
    private readonly IMapper mapper;
    private readonly ILogger<TodoListController> logger;
    private readonly int pageSize = 6;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListController"/> class.
    /// </summary>
    /// <param name="apiService">The to-do list web API service.</param>
    /// <param name="mapper">The mapper.</param>
    public TodoListController(ITodoListWebApiService apiService, IMapper mapper, ILogger<TodoListController> logger)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Gets the index view of the to-do list.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <returns>The index view of the to-do list.</returns>
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        if (!this.User.Identity!.IsAuthenticated)
        {
            return this.RedirectToAction("Login", "Account");
        }

        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.apiService.GetPagedListOfTodoListsAsync(token, page, this.pageSize);

        this.TempData["CurrentPage"] = page;
        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Get the view of to-do lists", null);
        return this.View(this.mapper.Map<PagedModel<TodoListModel>>(pagedResult));
    }

    /// <summary>
    /// Gets the create view for a new to-do list.
    /// </summary>
    /// <returns>The create view for a new to-do list.</returns>
    [HttpGet]
    [Route("create")]
    public IActionResult Create()
    {
        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Get the view of creating to-do list", null);
        return this.View(new TodoListModel());
    }

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="model">The to-do list model.</param>
    /// <returns>The action result after creating the to-do list.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("create")]
    public async Task<IActionResult> Create(TodoListModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newTodoList = this.mapper.Map<TodoList>(model);

        _ = await this.apiService.CreateTodoListAsync(token, newTodoList);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully create to-do list with id = {newTodoList.Id}", null);
        return this.RedirectToAction(nameof(this.Index));
    }

    /// <summary>
    /// Gets the edit view for a to-do list.
    /// </summary>
    /// <param name="id">The to-do list ID.</param>
    /// <returns>The edit view for the to-do list.</returns>
    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var todoListToEdit = await this.apiService.GetTodoListByIdAsync(token, id);

        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Get the view of editing to-do lists", null);
        return this.View(this.mapper.Map<TodoListModel>(todoListToEdit));
    }

    /// <summary>
    /// Edits a to-do list.
    /// </summary>
    /// <param name="id">The to-do list ID.</param>
    /// <param name="model">The to-do list model.</param>
    /// <returns>The action result after editing the to-do list.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int id, TodoListModel model)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var newTodoList = this.mapper.Map<TodoList>(model);

        await this.apiService.UpdateTodoListAsync(token, id, newTodoList);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully edit to-do list with id = {newTodoList.Id}", null);
        return this.RedirectToAction(nameof(this.Index));
    }

    /// <summary>
    /// Gets the delete view for a to-do list.
    /// </summary>
    /// <param name="id">The to-do list ID.</param>
    /// <returns>The delete view for the to-do list.</returns>
    [HttpGet]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var todoListToDelete = await this.apiService.GetTodoListByIdAsync(token, id);

        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Get the view of deleting to-do lists", null);
        return this.View(this.mapper.Map<TodoListModel>(todoListToDelete));
    }

    /// <summary>
    /// Deletes a to-do list.
    /// </summary>
    /// <param name="id">The to-do list ID.</param>
    /// <returns>The action result after deleting the to-do list.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.DeleteTodoListAsync(token, id);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesfully delete to-do list with id = {id}", null);
        return this.RedirectToAction(nameof(this.Index));
    }
}

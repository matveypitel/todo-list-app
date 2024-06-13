using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("todolists")]
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService apiService;
    private readonly IMapper mapper;

    public TodoListController(ITodoListWebApiService apiService, IMapper mapper)
    {
        this.apiService = apiService;
        this.mapper = mapper;
    }

    public int PageSize { get; set; } = 8;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.apiService.GetPagedListOfTodoListsAsync(token, page, this.PageSize);

        return this.View(this.mapper.Map<PagedModel<TodoListModel>>(pagedResult));
    }

    [HttpGet]
    [Route("create")]
    public IActionResult Create()
    {
        return this.View(new TodoListModel());
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(TodoListModel model)
    {
        var token = TokenUtility.GetToken(this.Request);
        var userName = this.User.Identity!.Name ?? string.Empty;

        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var newTodoList = this.mapper.Map<TodoList>(model);
        newTodoList.Owner = userName;

        _ = await this.apiService.CreateTodoListAsync(token, newTodoList);

        return this.RedirectToAction(nameof(this.Index));
    }

    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var todoListToEdit = await this.apiService.GetTodoListByIdAsync(token, id);

        return this.View(this.mapper.Map<TodoListModel>(todoListToEdit));
    }

    [HttpPost]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(int id, TodoListModel model)
    {
        var token = TokenUtility.GetToken(this.Request);
        var userName = this.User.Identity!.Name ?? string.Empty;

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var newTodoList = this.mapper.Map<TodoList>(model);
        newTodoList.Owner = userName;

        await this.apiService.UpdateTodoListAsync(token, id, newTodoList);

        return this.RedirectToAction(nameof(this.Index));
    }

    [HttpGet]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var todoListToDelete = await this.apiService.GetTodoListByIdAsync(token, id);

        return this.View(this.mapper.Map<TodoListModel>(todoListToDelete));
    }

    [HttpPost]
    [Route("deleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        await this.apiService.DeleteTodoListAsync(token, id);

        return this.RedirectToAction(nameof(this.Index));
    }
}

using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Abstractions;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolists")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITodoListDatabaseService databaseService;

    public TodoListController(IMapper mapper, ITodoListDatabaseService databaseService)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
    }

    [HttpPost]
    public async Task<ActionResult<TodoListModel>> CreateTodoList([FromBody] TodoListModel todoListModel)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var newtodoList = this.mapper.Map<TodoList>(todoListModel);
        newtodoList.UserId = userId;
        var todoList = await this.databaseService.CreateTodoListAsync(newtodoList);

        return this.CreatedAtAction(nameof(this.GetTodoListDetails), new { id = todoList.Id }, this.mapper.Map<TodoListModel>(todoList));
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TodoListModel>>> GetTodoLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var todoLists = await this.databaseService.GetPagedListOfTodoListsAsync(userId, page, pageSize);

        if (todoLists.TotalCount != 0 && page > (int)Math.Ceiling((double)todoLists.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TodoListModel>>(todoLists));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListModel>> GetTodoListDetails([FromRoute] int id)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var todoList = await this.databaseService.GetTodoListByIdAsync(id, userId);

        return this.Ok(this.mapper.Map<TodoListModel>(todoList));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoList([FromRoute] int id, [FromBody] TodoListModel todoListModel)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var todoList = this.mapper.Map<TodoList>(todoListModel);
        todoList.UserId = userId;

        await this.databaseService.UpdateTodoListAsync(id, todoList);

        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoList([FromRoute] int id)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        await this.databaseService.DeleteTodoListAsync(id, userId);

        return this.NoContent();
    }
}

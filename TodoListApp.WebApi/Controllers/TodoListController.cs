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
    public async Task<ActionResult<IEnumerable<TodoListModel>>> GetTodoLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var totalCount = await this.databaseService.GetCountAsync(userId);

        if (totalCount != 0 && page > (int)Math.Ceiling((double)totalCount / pageSize))
        {
            return this.BadRequest();
        }

        var todoLists = await this.databaseService.GetListOfTodoListsAsync(userId, page, pageSize);

        var result = new PagedResult<TodoListModel>
        {
            Items = this.mapper.Map<IEnumerable<TodoListModel>>(todoLists),
            TotalCount = totalCount,
            ItemsPerPage = pageSize,
            CurrentPage = page,
        };

        return this.Ok(result);
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

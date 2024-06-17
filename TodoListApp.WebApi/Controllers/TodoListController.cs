using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

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
        var userName = this.GetUserName();

        var newtodoList = this.mapper.Map<TodoList>(todoListModel);
        var todoList = await this.databaseService.CreateTodoListAsync(newtodoList, userName);

        return this.CreatedAtAction(
            nameof(this.GetTodoListDetails),
            new { id = todoList.Id },
            this.mapper.Map<TodoListModel>(todoList));
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TodoListModel>>> GetTodoLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var todoLists = await this.databaseService.GetPagedListOfTodoListsAsync(userName, page, pageSize);

        if (todoLists.TotalCount != 0 && page > (int)Math.Ceiling((double)todoLists.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TodoListModel>>(todoLists));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListModel>> GetTodoListDetails([FromRoute] int id)
    {
        var userName = this.GetUserName();

        var todoList = await this.databaseService.GetTodoListByIdAsync(id, userName);

        return this.Ok(this.mapper.Map<TodoListModel>(todoList));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoList([FromRoute] int id, [FromBody] TodoListModel todoListModel)
    {
        var userName = this.GetUserName();

        var todoList = this.mapper.Map<TodoList>(todoListModel);

        await this.databaseService.UpdateTodoListAsync(id, todoList, userName);

        return this.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoList([FromRoute] int id)
    {
        var userName = this.GetUserName();

        await this.databaseService.DeleteTodoListAsync(id, userName);

        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

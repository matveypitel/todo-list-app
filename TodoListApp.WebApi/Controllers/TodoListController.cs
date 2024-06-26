using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing to-do lists.
/// </summary>
[Authorize]
[Route("api/todolists")]
[ApiController]
public class TodoListController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITodoListDatabaseService databaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListController"/> class.
    /// </summary>
    /// <param name="mapper">The mapper instance.</param>
    /// <param name="databaseService">The database service instance.</param>
    public TodoListController(IMapper mapper, ITodoListDatabaseService databaseService)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
    }

    /// <summary>
    /// POST: api/todolists.
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="todoListModel">The to-do list model.</param>
    /// <returns>The created to-do list.</returns>
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

    /// <summary>
    /// GET: api/todolists.
    /// Gets a paged list of to-do lists.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of to-do lists.</returns>
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

    /// <summary>
    /// GET: api/todolists/{id}.
    /// Gets the details of a to-do list by its ID.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The details of the to-do list.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListModel>> GetTodoListDetails([FromRoute] int id)
    {
        var userName = this.GetUserName();

        var todoList = await this.databaseService.GetTodoListByIdAsync(id, userName);

        return this.Ok(this.mapper.Map<TodoListModel>(todoList));
    }

    /// <summary>
    /// PUT: api/todolists/{id}.
    /// Updates a to-do list.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="todoListModel">The updated to-do list model.</param>
    /// <returns>The updated to-do list.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoList([FromRoute] int id, [FromBody] TodoListModel todoListModel)
    {
        var userName = this.GetUserName();

        var todoList = this.mapper.Map<TodoList>(todoListModel);

        await this.databaseService.UpdateTodoListAsync(id, todoList, userName);

        return this.NoContent();
    }

    /// <summary>
    /// DELETE: api/todolists/{id}.
    /// Deletes a to-do list by its ID.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>No content.</returns>
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

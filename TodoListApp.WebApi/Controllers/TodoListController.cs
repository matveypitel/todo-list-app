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
    private static readonly Action<ILogger, string, string, Exception> LogError = LoggerMessage.Define<string, string>(
        LogLevel.Error,
        default,
        "Produce error response: [Error {Code}] {ExMessage}");

    private readonly IMapper mapper;
    private readonly ITodoListDatabaseService databaseService;
    private readonly ILogger<TodoListController> logger;

    public TodoListController(IMapper mapper, ITodoListDatabaseService databaseService, ILogger<TodoListController> logger)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<TodoListModel>> CreateTodoList([FromBody] TodoListModel todoListModel)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var newtodoList = this.mapper.Map<TodoList>(todoListModel);
            newtodoList.UserId = userId;
            var todoList = await this.databaseService.CreateAsync(newtodoList);

            return this.CreatedAtAction(nameof(this.GetTodoList), new { id = todoList.Id }, this.mapper.Map<TodoListModel>(todoList));
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListModel>>> GetTodoLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var totalCount = await this.databaseService.GetCountAsync(userId);

            if (totalCount != 0 && page > (int)Math.Ceiling((double)totalCount / pageSize))
            {
                return this.BadRequest();
            }

            var todoLists = await this.databaseService.GetAllAsync(userId, page, pageSize);

            var result = new PagedResult<TodoListModel>
            {
                Items = this.mapper.Map<IEnumerable<TodoListModel>>(todoLists),
                TotalCount = totalCount,
                ItemsPerPage = pageSize,
                CurrentPage = page,
            };

            return this.Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListModel>> GetTodoList([FromRoute] int id)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var todoList = await this.databaseService.GetByIdAsync(id, userId);

            return this.Ok(this.mapper.Map<TodoListModel>(todoList));
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTodoList([FromRoute] int id, [FromBody] TodoListModel todoListModel)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var todoList = this.mapper.Map<TodoList>(todoListModel);
            todoList.UserId = userId;

            await this.databaseService.UpdateAsync(id, todoList);

            return this.NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoList([FromRoute] int id)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            await this.databaseService.DeleteAsync(id, userId);

            return this.NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            LogError(this.logger, "404", ex.Message, ex);

            return this.NotFound();
        }
        catch (ArgumentNullException ex)
        {
            LogError(this.logger, "400", ex.Message, ex);

            return this.BadRequest();
        }
        catch (Exception ex) when (ex is not KeyNotFoundException && ex is not ArgumentNullException)
        {
            LogError(this.logger, "500", ex.Message, ex);

            return this.StatusCode(500);
        }
    }
}

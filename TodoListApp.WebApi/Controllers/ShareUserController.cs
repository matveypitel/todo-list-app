using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing shared users of a to-do list.
/// </summary>
[Authorize]
[Route("api/todolists/{todoListId}/share_users")]
[ApiController]
public class ShareUserController : ControllerBase
{
    private readonly IShareUserDatabaseService databaseService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShareUserController"/> class.
    /// </summary>
    /// <param name="databaseService">The database service for managing shared users.</param>
    /// <param name="mapper">The mapper for mapping between DTOs and domain models.</param>
    public ShareUserController(IShareUserDatabaseService databaseService, IMapper mapper)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/share_users.
    /// Get a paged list of shared users for a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The paged list of shared users.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedModel<TodoListUserModel>>> GetUsers([FromRoute] int todoListId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var users = await this.databaseService.GetPagedTodoListUsersListAsync(todoListId, userName, page, pageSize);

        if (users.TotalCount != 0 && page > (int)Math.Ceiling((double)users.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TodoListUserModel>>(users));
    }

    /// <summary>
    /// GET: api/todolists/{todoListId}/share_users/{userName}.
    /// Get a shared user of a to-do list by username.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <returns>The shared user.</returns>
    [HttpGet("{userName}")]
    public async Task<ActionResult<TodoListUserModel>> GetTodoListUser([FromRoute] int todoListId, [FromRoute] string userName)
    {
        var userNameRequester = this.GetUserName();

        var user = await this.databaseService.GetUserByNameAsync(todoListId, userNameRequester, userName);

        return this.Ok(this.mapper.Map<TodoListUserModel>(user));
    }

    /// <summary>
    /// POST: api/todolists/{todoListId}/share_users.
    /// Add a user to a to-do list as a shared user.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="model">The user model to add.</param>
    /// <returns>The added user.</returns>
    [HttpPost]
    public async Task<ActionResult> AddUserToTodoList([FromRoute] int todoListId, [FromBody] TodoListUserModel model)
    {
        var userNameRequester = this.GetUserName();

        if (model == null || model.UserName == null)
        {
            return this.BadRequest();
        }

        var todoListUser = await this.databaseService.AddUserToTodoListAsync(todoListId, userNameRequester, model.UserName, model.Role);

        return this.CreatedAtAction(
            nameof(this.GetTodoListUser),
            new { todoListId, userName = todoListUser.UserName },
            this.mapper.Map<TodoListUserModel>(todoListUser));
    }

    /// <summary>
    /// PUT: api/todolists/{todoListId}/share_users/{userName}.
    /// Update the role of a shared user in a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <param name="model">The updated user model.</param>
    /// <returns>No content.</returns>
    [HttpPut("{userName}")]
    public async Task<ActionResult> UpdateUserRole([FromRoute] int todoListId, [FromRoute] string userName, [FromBody] TodoListUserModel model)
    {
        var userNameRequester = this.GetUserName();

        if (model == null)
        {
            return this.BadRequest();
        }

        await this.databaseService.UpdateUserRoleAsync(todoListId, userNameRequester, userName, model.Role);

        return this.NoContent();
    }

    /// <summary>
    /// DELETE: api/todolists/{todoListId}/share_users/{userName}.
    /// Remove a shared user from a to-do list.
    /// </summary>
    /// <param name="todoListId">The ID of the todo list.</param>
    /// <param name="userName">The username of the shared user.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{userName}")]
    public async Task<ActionResult> RemoveUserFromTodoList([FromRoute] int todoListId, [FromRoute] string userName)
    {
        var userNameRequester = this.GetUserName();

        await this.databaseService.RemoveUserFromTodoListAsync(todoListId, userNameRequester, userName);

        return this.NoContent();
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

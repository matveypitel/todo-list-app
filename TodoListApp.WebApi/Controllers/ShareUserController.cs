using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolists/{todoListId}/share_users")]
[ApiController]
public class ShareUserController : ControllerBase
{
    private readonly IShareUserDatabaseService databaseService;
    private readonly IMapper mapper;

    public ShareUserController(IShareUserDatabaseService databaseService, IMapper mapper)
    {
        this.databaseService = databaseService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TodoListUserModel>>> GetUsers([FromRoute] int todoListId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var users = await this.databaseService.GetPagedTodoListUsersListAsync(todoListId, userName, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TodoListUserModel>>(users));
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult<TodoListUserModel>> GetTodoListUser([FromRoute] int todoListId, [FromRoute] string userName)
    {
        var userNameRequester = this.GetUserName();

        var user = await this.databaseService.GetUserByNameAsync(todoListId, userNameRequester, userName);

        return this.Ok(this.mapper.Map<TodoListUserModel>(user));
    }

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

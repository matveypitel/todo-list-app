using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Abstractions;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/tasks/assigned_to_me")]
[ApiController]
public class AssignedTaskController : ControllerBase
{
    private static readonly Action<ILogger, string, string, Exception> LogError = LoggerMessage.Define<string, string>(
        LogLevel.Error,
        default,
        "Produce error response: [Error {Code}] {ExMessage}");

    private readonly IMapper mapper;
    private readonly ILogger<AssignedTaskController> logger;
    private readonly ITaskItemDatabaseService databaseService;

    public AssignedTaskController(ITaskItemDatabaseService databaseService, IMapper mapper, ILogger<AssignedTaskController> logger)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemModel>>> GetAssignedTaskItems([FromQuery] string? status, [FromQuery] string? sort)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            var taskItems = await this.databaseService.GetAssignedToUserAsync(userId, status, sort);

            return this.Ok(this.mapper.Map<IEnumerable<TaskItemModel>>(taskItems));
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

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string status)
    {
        try
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            await this.databaseService.UpdateTaskStatusAsync(id, userId, status);

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

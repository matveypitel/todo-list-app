using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Abstractions;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/tasks/assigned_to_me")]
[ApiController]
public class AssignedTaskController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITaskItemDatabaseService databaseService;

    public AssignedTaskController(ITaskItemDatabaseService databaseService, IMapper mapper)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> GetAssignedTasksToUser(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? sort = null)
    {
        var userName = this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        var taskItems = await this.databaseService.GetPagedListOfAssignedTaskToUserAsync(userName, page, pageSize, status, sort);

        if (taskItems.TotalCount != 0 && page > (int)Math.Ceiling((double)taskItems.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(taskItems));
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateTaskStatus([FromRoute] int id, [FromBody] string status)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        await this.databaseService.UpdateTaskStatusAsync(id, userId, status);

        return this.NoContent();
    }
}

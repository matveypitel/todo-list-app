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
    private readonly IMapper mapper;
    private readonly ITaskItemDatabaseService databaseService;

    public AssignedTaskController(ITaskItemDatabaseService databaseService, IMapper mapper)
    {
        this.mapper = mapper;
        this.databaseService = databaseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemModel>>> GetAssignedTasksToUser([FromQuery] string? status, [FromQuery] string? sort)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        var taskItems = await this.databaseService.GetAssignedTaskToUserAsync(userId, status, sort);

        return this.Ok(this.mapper.Map<IEnumerable<TaskItemModel>>(taskItems));
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string status)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        await this.databaseService.UpdateTaskStatusAsync(id, userId, status);

        return this.NoContent();
    }
}

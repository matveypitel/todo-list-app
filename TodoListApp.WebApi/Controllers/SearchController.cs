using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITaskDatabaseService taskDatabaseService;

    public SearchController(IMapper mapper, ITaskDatabaseService taskDatabaseService)
    {
        this.mapper = mapper;
        this.taskDatabaseService = taskDatabaseService;
    }

    [HttpGet]
    [Route("tasks")]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> SearchTasks(
        [FromQuery] string? title,
        [FromQuery] DateTime? creationDate,
        [FromQuery] DateTime? dueDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var tasks = await this.taskDatabaseService.GetPagedListOfTasksSearchResultsAsync(userName, title, creationDate, dueDate, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(tasks));
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

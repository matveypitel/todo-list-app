using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for searching tasks.
/// </summary>
[Authorize]
[Route("api/search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITaskDatabaseService taskDatabaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchController"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="taskDatabaseService">The task database service.</param>
    public SearchController(IMapper mapper, ITaskDatabaseService taskDatabaseService)
    {
        this.mapper = mapper;
        this.taskDatabaseService = taskDatabaseService;
    }

    /// <summary>
    /// GET: api/search/tasks.
    /// Searches tasks based on the specified criteria.
    /// </summary>
    /// <param name="title">The title of the task.</param>
    /// <param name="creationDate">The creation date of the task.</param>
    /// <param name="dueDate">The due date of the task.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of task items.</returns>
    [HttpGet]
    [Route("tasks")]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> SearchTasks(
        [FromQuery] string? title,
        [FromQuery] string? creationDate,
        [FromQuery] string? dueDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var tasks = await this.taskDatabaseService.GetPagedListOfTasksSearchResultsAsync(userName, title, creationDate, dueDate, page, pageSize);

        if (tasks.TotalCount != 0 && page > (int)Math.Ceiling((double)tasks.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(tasks));
    }

    private string GetUserName()
    {
        return this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}

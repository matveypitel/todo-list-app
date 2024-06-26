using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

/// <summary>
/// Controller for managing tags.
/// </summary>
[Authorize]
[Route("api/tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITagDatabaseService tagDatabaseService;
    private readonly ITaskDatabaseService taskDatabaseService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagController"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="tagDatabaseService">The tag database service.</param>
    /// <param name="taskDatabaseService">The task database service.</param>
    public TagController(IMapper mapper, ITagDatabaseService tagDatabaseService, ITaskDatabaseService taskDatabaseService)
    {
        this.mapper = mapper;
        this.tagDatabaseService = tagDatabaseService;
        this.taskDatabaseService = taskDatabaseService;
    }

    /// <summary>
    /// GET: api/tags.
    /// Gets all tags.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of tags.</returns>
    [HttpGet]
    public async Task<ActionResult<PagedModel<TagModel>>> GetAllTags([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var tags = await this.tagDatabaseService.GetPagedListOfAllAsync(userName, page, pageSize);

        if (tags.TotalCount != 0 && page > (int)Math.Ceiling((double)tags.TotalCount / pageSize))
        {
            return this.BadRequest();
        }

        return this.Ok(this.mapper.Map<PagedModel<TagModel>>(tags));
    }

    /// <summary>
    /// GET: api/tags/tasks.
    /// Gets tasks with the specified tag.
    /// </summary>
    /// <param name="tag">The tag.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paged list of tasks with the specified tag.</returns>
    [HttpGet]
    [Route("tasks")]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> GetTasksWithTag(
        [FromQuery] string tag,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userName = this.GetUserName();

        var tasks = await this.taskDatabaseService.GetPagedListOfTasksWithTagAsync(userName, tag, page, pageSize);

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

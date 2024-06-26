using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing tags.
/// </summary>
[Authorize]
[Route("tags")]
public class TagController : Controller
{
    private readonly ITagWebApiService tagWebApiService;
    private readonly IMapper mapper;
    private readonly int pageSize = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagController"/> class.
    /// </summary>
    /// <param name="tagWebApiService">The tag web API service.</param>
    /// <param name="mapper">The mapper.</param>
    public TagController(ITagWebApiService tagWebApiService, IMapper mapper)
    {
        this.tagWebApiService = tagWebApiService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets the index page for tags.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <returns>The index view.</returns>
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.tagWebApiService.GetAllTagsAsync(token, page, this.pageSize);

        return this.View(this.mapper.Map<PagedModel<TagModel>>(pagedResult));
    }

    /// <summary>
    /// Gets the tasks with the specified tag.
    /// </summary>
    /// <param name="tag">The tag.</param>
    /// <param name="page">The page number.</param>
    /// <returns>The tasks view.</returns>
    [HttpGet]
    [Route("tasks")]
    public async Task<IActionResult> TasksWithTag([FromQuery] string tag, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.tagWebApiService.GetTasksWithTag(token, tag, page, this.pageSize);

        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }
}

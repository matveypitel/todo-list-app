using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApi.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/tags")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ITagDatabaseService tagDatabaseService;
    private readonly ITaskDatabaseService taskDatabaseService;

    public TagController(IMapper mapper, ITagDatabaseService tagDatabaseService, ITaskDatabaseService taskDatabaseService)
    {
        this.mapper = mapper;
        this.tagDatabaseService = tagDatabaseService;
        this.taskDatabaseService = taskDatabaseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedModel<TagModel>>> GetAllTags([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userName = this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        var tags = await this.tagDatabaseService.GetPagedListOfAllAsync(userName, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TagModel>>(tags));
    }

    [HttpGet]
    [Route("tasks")]
    public async Task<ActionResult<PagedModel<TaskItemModel>>> GetTasksWithTag(
        [FromQuery] string tag,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userName = this.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        var tasks = await this.taskDatabaseService.GetPagedListOfTasksWithTagAsync(userName, tag, page, pageSize);

        return this.Ok(this.mapper.Map<PagedModel<TaskItemModel>>(tasks));
    }
}

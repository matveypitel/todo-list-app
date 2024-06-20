using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("tags")]
public class TagController : Controller
{
    private readonly ITagWebApiService tagWebApiService;
    private readonly IMapper mapper;

    public TagController(ITagWebApiService tagWebApiService, IMapper mapper)
    {
        this.tagWebApiService = tagWebApiService;
        this.mapper = mapper;
    }

    public int PageSize { get; set; } = 8;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.tagWebApiService.GetAllTagsAsync(token, page, this.PageSize);

        return this.View(this.mapper.Map<PagedModel<TagModel>>(pagedResult));
    }

    [HttpGet]
    [Route("tasks")]
    public async Task<IActionResult> TasksWithTag([FromQuery] string tag, [FromQuery] int page = 1)
    {
        var token = TokenUtility.GetToken(this.Request);
        var pagedResult = await this.tagWebApiService.GetTasksWithTag(token, tag, page, this.PageSize);

        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }
}

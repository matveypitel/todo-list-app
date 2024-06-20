using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("search")]
public class SearchController : Controller
{

    private readonly ITaskWebApiService apiService;
    private readonly IMapper mapper;

    public SearchController(ITaskWebApiService apiService, IMapper mapper)
    {
        this.apiService = apiService;
        this.mapper = mapper;
    }

    public int PageSize { get; set; } = 10;

    [HttpGet]
    [Route("tasks")]
    public async Task<IActionResult> Index([FromQuery] int page = 1, string? title = null, string? creationDate = null, string? dueDate = null)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedSearchedTaskAsync(token, page, this.PageSize, title, creationDate, dueDate);

        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }
}

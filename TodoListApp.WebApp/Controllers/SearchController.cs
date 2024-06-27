using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for searching tasks.
/// </summary>
[Authorize]
[Route("search")]
public class SearchController : Controller
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly ITaskWebApiService apiService;
    private readonly IMapper mapper;
    private readonly int pageSize = 10;
    private readonly ILogger<SearchController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchController"/> class.
    /// </summary>
    /// <param name="apiService">The task web API service.</param>
    /// <param name="mapper">The mapper.</param>
    public SearchController(ITaskWebApiService apiService, IMapper mapper, ILogger<SearchController> logger)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Gets the search results for tasks.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="title">The task title.</param>
    /// <param name="creationDate">The task creation date.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <returns>The search results as a view.</returns>
    [HttpGet]
    [Route("tasks")]
    public async Task<IActionResult> Index([FromQuery] int page = 1, string? title = null, string? creationDate = null, string? dueDate = null)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetPagedSearchedTaskAsync(token, page, this.pageSize, title, creationDate, dueDate);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning view of {pagedResult.TotalCount} searched tasks with filters", null);
        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }
}

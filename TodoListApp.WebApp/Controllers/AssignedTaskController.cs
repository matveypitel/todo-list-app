using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Represents a controller for managing assigned tasks.
/// </summary>
[Authorize]
[Route("tasks/assigned_to_me")]
public class AssignedTaskController : Controller
{
    private static readonly Action<ILogger, string, string, Exception?> LogInformation =
        LoggerMessage.Define<string, string>(
            LogLevel.Information,
            default,
            "{DateTime} Message: [{Message}]");

    private readonly ITaskWebApiService apiService;
    private readonly IMapper mapper;
    private readonly int pageSize = 10;
    private readonly ILogger<AssignedTaskController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignedTaskController"/> class.
    /// </summary>
    /// <param name="apiService">The task web API service.</param>
    /// <param name="mapper">The mapper.</param>
    public AssignedTaskController(ITaskWebApiService apiService, IMapper mapper, ILogger<AssignedTaskController> logger)
    {
        this.apiService = apiService;
        this.mapper = mapper;
        this.logger = logger;
    }

    /// <summary>
    /// Gets the assigned tasks for the current user.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="status">The task status.</param>
    /// <param name="sort">The sort order.</param>
    /// <returns>The view containing the assigned tasks.</returns>
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1, string? status = null, string? sort = null)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetAssignedTasksToUserAsync(token, page, this.pageSize, status, sort);
        this.TempData["CurrentPage"] = page;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Returning view of {pagedResult.TotalCount} assigned tasks", null);
        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }

    /// <summary>
    /// Gets the status of an assigned task by its ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The view containing the task status.</returns>
    [HttpGet]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskItem = await this.apiService.GetAssignedTaskByIdAsync(token, id);
        this.ViewBag.CurrentPage = this.TempData["CurrentPage"] ?? 1;

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Get change status page of assigned task with id = {id}", null);
        return this.View(this.mapper.Map<TaskItemModel>(taskItem));
    }

    /// <summary>
    /// Updates the status of an assigned task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="taskItemModel">The updated task item model.</param>
    /// <returns>The redirect action result.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(int id, TaskItemModel taskItemModel, int currentPage)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var updatedTask = this.mapper.Map<TaskItem>(taskItemModel);

        await this.apiService.UpdateTaskStatusAsync(token, id, updatedTask);

        LogInformation(this.logger, DateTime.Now.ToString(CultureInfo.InvariantCulture), $"Succesful change status of task with id = {id}, redirecting to paged view", null);
        return this.RedirectToAction(nameof(this.Index), new { page = currentPage });
    }
}

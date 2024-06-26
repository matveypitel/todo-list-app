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
    private readonly ITaskWebApiService apiService;
    private readonly IMapper mapper;
    private readonly int pageSize = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssignedTaskController"/> class.
    /// </summary>
    /// <param name="apiService">The task web API service.</param>
    /// <param name="mapper">The mapper.</param>
    public AssignedTaskController(ITaskWebApiService apiService, IMapper mapper)
    {
        this.apiService = apiService;
        this.mapper = mapper;
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
    public async Task<IActionResult> Status(int id, TaskItemModel taskItemModel)
    {
        var token = TokenUtility.GetToken(this.Request);

        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var updatedTask = this.mapper.Map<TaskItem>(taskItemModel);

        await this.apiService.UpdateTaskStatusAsync(token, id, updatedTask);

        return this.RedirectToAction(nameof(this.Index));
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models.Domains;
using TodoListApp.Models.DTOs;
using TodoListApp.WebApp.Interfaces;
using TodoListApp.WebApp.Utilities;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("tasks/assigned_to_me")]
public class AssignedTaskController : Controller
{
    private readonly ITaskWebApiService apiService;
    private readonly IMapper mapper;

    public AssignedTaskController(ITaskWebApiService apiService, IMapper mapper)
    {
        this.apiService = apiService;
        this.mapper = mapper;
    }

    public int PageSize { get; set; } = 10;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int page = 1, string? status = null, string? sort = null)
    {
        var token = TokenUtility.GetToken(this.Request);

        var pagedResult = await this.apiService.GetAssignedTasksToUserAsync(token, page, this.PageSize, status, sort);

        return this.View(this.mapper.Map<PagedModel<TaskItemModel>>(pagedResult));
    }

    [HttpGet]
    [Route("status/{id}")]
    public async Task<IActionResult> Status(int id)
    {
        var token = TokenUtility.GetToken(this.Request);

        var taskItem = await this.apiService.GetAssignedTaskByIdAsync(token, id);

        return this.View(this.mapper.Map<TaskItemModel>(taskItem));
    }

    [HttpPost]
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

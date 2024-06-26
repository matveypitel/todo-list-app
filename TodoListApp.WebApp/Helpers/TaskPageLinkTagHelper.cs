using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating page links for task items.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-task-model")]
public class TaskPageLinkTagHelper : PageLinkTagHelper<TaskItemModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskPageLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public TaskPageLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating page links for assigned tasks.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-assignedTask-model")]
public class AssignedTaskPageLinkTagHelper : PageLinkTagHelper<TaskItemModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignedTaskPageLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public AssignedTaskPageLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating links to to-do list pages.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-todoList-model")]
public class TodoListLinkTagHelper : PageLinkTagHelper<TodoListModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public TodoListLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

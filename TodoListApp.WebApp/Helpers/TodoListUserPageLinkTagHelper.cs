using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating page links for TodoListUserModel.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-todoList-user-model")]
public class TodoListUserPageLinkTagHelper : PageLinkTagHelper<TodoListUserModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListUserPageLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public TodoListUserPageLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

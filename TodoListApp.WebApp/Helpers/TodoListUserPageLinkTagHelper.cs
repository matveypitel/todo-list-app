using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

[HtmlTargetElement("div", Attributes = "is-todoList-user-model")]
public class TodoListUserPageLinkTagHelper : PageLinkTagHelper<TodoListUserModel>
{
    public TodoListUserPageLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

[HtmlTargetElement("div", Attributes = "is-task-model")]
public class TaskPageLinkTagHelper : PageLinkTagHelper<TaskItemModel>
{
    public TaskPageLinkTagHelper(IUrlHelperFactory helperFactory)
    : base(helperFactory)
    {
    }
}

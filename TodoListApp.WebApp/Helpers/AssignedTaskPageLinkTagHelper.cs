using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

[HtmlTargetElement("div", Attributes = "is-assignedTask-model")]
public class AssignedTaskPageLinkTagHelper : PageLinkTagHelper<TaskItemModel>
{
    public AssignedTaskPageLinkTagHelper(IUrlHelperFactory helperFactory)
    : base(helperFactory)
    {
    }
}

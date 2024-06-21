using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

[HtmlTargetElement("div", Attributes = "is-comment-model")]
public class CommentLinkTagHelper : PageLinkTagHelper<CommentModel>
{
    public CommentLinkTagHelper(IUrlHelperFactory helperFactory)
    : base(helperFactory)
    {
    }
}

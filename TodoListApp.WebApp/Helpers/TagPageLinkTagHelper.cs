using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

[HtmlTargetElement("div", Attributes = "is-tag-model")]
public class TagPageLinkTagHelper : PageLinkTagHelper<TagModel>
{
    public TagPageLinkTagHelper(IUrlHelperFactory helperFactory)
    : base(helperFactory)
    {
    }
}

using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating page links for a specific tag model.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-tag-model")]
public class TagPageLinkTagHelper : PageLinkTagHelper<TagModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagPageLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public TagPageLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

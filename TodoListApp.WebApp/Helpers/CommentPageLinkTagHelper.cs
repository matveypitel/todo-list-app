using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Represents a tag helper for generating comment page links.
/// </summary>
[HtmlTargetElement("div", Attributes = "is-comment-model")]
public class CommentLinkTagHelper : PageLinkTagHelper<CommentModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentLinkTagHelper"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public CommentLinkTagHelper(IUrlHelperFactory helperFactory)
        : base(helperFactory)
    {
    }
}

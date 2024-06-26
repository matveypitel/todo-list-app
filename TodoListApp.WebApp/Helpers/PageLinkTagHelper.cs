using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.Models.DTOs;

namespace TodoListApp.WebApp.Helpers;

/// <summary>
/// Tag helper for generating pagination links.
/// </summary>
/// <typeparam name="T">The type of the paged model.</typeparam>
[HtmlTargetElement("div", Attributes = "page-model")]
public class PageLinkTagHelper<T> : TagHelper
{
    private readonly IUrlHelperFactory urlHelperFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageLinkTagHelper{T}"/> class.
    /// </summary>
    /// <param name="helperFactory">The URL helper factory.</param>
    public PageLinkTagHelper(IUrlHelperFactory helperFactory)
    {
        this.urlHelperFactory = helperFactory;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }

    public PagedModel<T>? PageModel { get; set; }

    public string? PageAction { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (this.PageModel != null && this.PageModel.TotalPages != 1 && this.ViewContext != null && output != null)
        {
            IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "pagination-container");

            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination pagination-sm justify-content-center mt-2");

            for (int i = 1; i <= this.PageModel.TotalPages; i++)
            {
                TagBuilder pageItem = this.CreateTag(urlHelper, i);
                _ = tag.InnerHtml.AppendHtml(pageItem);
            }

            _ = output.Content.AppendHtml(tag);
        }
    }

    private TagBuilder CreateTag(IUrlHelper urlHelper, int pageNumber = 1)
    {
        TagBuilder item = new TagBuilder("li");
        TagBuilder link = new TagBuilder("a");
        if (pageNumber == this.PageModel?.CurrentPage)
        {
            item.AddCssClass("active");
        }
        else
        {
            link.Attributes["href"] = urlHelper.Action(this.PageAction, new { page = pageNumber });
        }

        item.AddCssClass("page-item");
        link.AddCssClass("page-link");

        _ = link.InnerHtml.Append(pageNumber.ToString(CultureInfo.InvariantCulture));
        _ = item.InnerHtml.AppendHtml(link);
        return item;
    }
}

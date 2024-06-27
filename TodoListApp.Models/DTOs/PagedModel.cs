namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a generic model for paginated data.
/// </summary>
/// <typeparam name="T">The type of the data items.</typeparam>
public class PagedModel<T>
{
    /// <summary>
    /// Gets or sets the items in the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int ItemsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((decimal)this.TotalCount / this.ItemsPerPage);
}

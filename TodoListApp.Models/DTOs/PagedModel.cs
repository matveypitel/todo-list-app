namespace TodoListApp.Models.DTOs;

/// <summary>
/// Represents a generic model for paginated data.
/// </summary>
/// <typeparam name="T">The type of the data items.</typeparam>
public class PagedModel<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int TotalCount { get; set; }

    public int ItemsPerPage { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages => (int)Math.Ceiling((decimal)this.TotalCount / this.ItemsPerPage);
}

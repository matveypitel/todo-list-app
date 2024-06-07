namespace TodoListApp.Models.DTOs;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int TotalCount { get; set; }

    public int ItemsPerPage { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages => (int)Math.Ceiling((decimal)this.TotalCount / this.ItemsPerPage);
}

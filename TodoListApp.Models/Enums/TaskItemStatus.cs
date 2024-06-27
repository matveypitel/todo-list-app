namespace TodoListApp.Models.Enums;

/// <summary>
/// Enumerates the possible statuses of a task item.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>
    /// The task item has not been started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The task item is in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The task item has been completed.
    /// </summary>
    Completed,
}

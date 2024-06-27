namespace TodoListApp.Models.Enums;

/// <summary>
/// Enumerates the possible roles a user can have within a to-do list.
/// </summary>
public enum TodoListRole
{
    /// <summary>
    /// Represents a viewer role.
    /// </summary>
    Viewer,

    /// <summary>
    /// Represents an editor role.
    /// </summary>
    Editor,

    /// <summary>
    /// Represents an owner role.
    /// </summary>
    Owner,
}

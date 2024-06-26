using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.ValidationAttributes;

/// <summary>
/// Specifies that a DateTime property must be a future date.
/// </summary>
public sealed class DueDateAttribute : ValidationAttribute
{
    /// <summary>
    /// Determines whether the specified value of the object is valid.
    /// </summary>
    /// <param name="value">The value of the object to validate.</param>
    /// <returns>true if the specified value is valid; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        if (value is not DateTime)
        {
            return true;
        }

        return (DateTime)value >= DateTime.Now;
    }

    /// <summary>
    /// Applies formatting to an error message, based on the data field where the error occurred.
    /// </summary>
    /// <param name="name">The name of the field that caused the validation failure.</param>
    /// <returns>An instance of the formatted error message.</returns>
    public override string FormatErrorMessage(string name)
    {
        return "The field " + name + " must be a future date.";
    }
}

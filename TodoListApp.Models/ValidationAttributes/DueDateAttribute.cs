using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Models.ValidationAttributes;
public sealed class DueDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime)
        {
            return true;
        }

        return (DateTime)value >= DateTime.Now;
    }

    public override string FormatErrorMessage(string name)
    {
        return "The field " + name + " must be a future date.";
    }
}

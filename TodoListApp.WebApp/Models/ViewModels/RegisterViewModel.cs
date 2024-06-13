using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

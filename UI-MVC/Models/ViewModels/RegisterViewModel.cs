namespace UI_MVC.Models.ViewModels;

public class RegisterViewModel
{
    public Guid UserUniqueCode { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
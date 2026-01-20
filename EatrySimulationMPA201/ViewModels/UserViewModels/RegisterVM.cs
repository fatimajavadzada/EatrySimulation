using System.ComponentModel.DataAnnotations;

namespace EatrySimulationMPA201.ViewModels.UserViewModels;
public class RegisterVM
{
    [Required, MinLength(3)]
    public string FullName { get; set; } = string.Empty;
    [Required, MinLength(3)]
    public string UserName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}

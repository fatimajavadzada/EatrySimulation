using System.ComponentModel.DataAnnotations;

namespace EatrySimulationMPA201.ViewModels.UserViewModels;
public class LoginVM
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    public bool IsRemember { get; set; }
}

using Microsoft.AspNetCore.Identity;

namespace EatrySimulationMPA201.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace EatrySimulationMPA201.ViewModels.ChefViewModels;
public class ChefCreateVM
{
    [Required, MinLength(3)]
    public string FullName { get; set; }
    [Required, MinLength(3)]
    public string Description { get; set; }
    [Required]
    public IFormFile ImagePath { get; set; }
    [Required]
    public int PositionId { get; set; }
}

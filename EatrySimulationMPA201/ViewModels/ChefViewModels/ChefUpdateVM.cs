using System.ComponentModel.DataAnnotations;

namespace EatrySimulationMPA201.ViewModels.ChefViewModels;

public class ChefUpdateVM
{
    public int Id { get; set; }
    [Required, MinLength(3)]
    public string FullName { get; set; }
    [Required, MinLength(3)]
    public string Description { get; set; }
    [Required]
    public int PositionId { get; set; }
    public IFormFile? ImagePath { get; set; }
}

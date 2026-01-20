using EatrySimulationMPA201.Models.Common;

namespace EatrySimulationMPA201.Models;

public class Chef : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
}

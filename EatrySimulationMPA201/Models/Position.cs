using EatrySimulationMPA201.Models.Common;

namespace EatrySimulationMPA201.Models;
public class Position : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Chef> Chefs { get; set; } = [];
}

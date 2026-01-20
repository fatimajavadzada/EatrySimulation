using EatrySimulationMPA201.Contexts;
using EatrySimulationMPA201.ViewModels.ChefViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EatrySimulationMPA201.Controllers;
public class HomeController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var chefs = await _context.Chefs.Select(x => new ChefGetVM()
        {
            Id = x.Id,
            FullName = x.FullName,
            Description = x.Description,
            ImagePath = x.ImagePath,
            PositionName = x.Position.Name
        }).ToListAsync();
        return View(chefs);
    }

}

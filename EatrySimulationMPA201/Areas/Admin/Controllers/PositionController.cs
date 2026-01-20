using EatrySimulationMPA201.Contexts;
using EatrySimulationMPA201.Models;
using EatrySimulationMPA201.ViewModels.PositionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EatrySimulationMPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles = "Admin")]
public class PositionController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var positions = await _context.Positions.Select(x => new PositionGetVM()
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync();
        return View(positions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Position newPosition = new()
        {
            Name = vm.Name
        };

        await _context.Positions.AddAsync(newPosition);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var existPosition = await _context.Positions.FindAsync(id);
        if (existPosition is null)
        {
            return NotFound();
        }

        PositionUpdateVM vm = new()
        {
            Id = existPosition.Id,
            Name = existPosition.Name
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(PositionUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existPosition = await _context.Positions.FindAsync(vm.Id);

        if (existPosition is null)
        {
            return NotFound();
        }

        existPosition.Name = vm.Name;
        _context.Positions.Update(existPosition);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        var existPosition = await _context.Positions.FindAsync(id);

        if (existPosition is null)
        {
            return NotFound();
        }

        _context.Positions.Remove(existPosition);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}

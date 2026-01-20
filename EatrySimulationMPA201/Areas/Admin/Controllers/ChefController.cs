using EatrySimulationMPA201.Contexts;
using EatrySimulationMPA201.Helpers;
using EatrySimulationMPA201.Models;
using EatrySimulationMPA201.ViewModels.ChefViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EatrySimulationMPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles = "Admin")]
public class ChefController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
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

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await SendPositionDataWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ChefCreateVM vm)
    {
        await SendPositionDataWithViewBag();

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existPosition = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);
        if (existPosition is false)
        {
            ModelState.AddModelError("PositionId", "Position is not found!");
            return View(vm);
        }

        if (!vm.ImagePath.CheckFileSize(2))
        {
            ModelState.AddModelError("ImagePath", "Image size must be less than 2MB!");
            return View(vm);
        }

        if (!vm.ImagePath.CheckFileType("image"))
        {
            ModelState.AddModelError("ImagePath", "Image size must be less than 2MB!");
            return View(vm);
        }

        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
        string imageName = vm.ImagePath.SaveFile(folderPath);

        Chef newChef = new()
        {
            FullName = vm.FullName,
            Description = vm.Description,
            PositionId = vm.PositionId,
            ImagePath = imageName
        };

        await _context.Chefs.AddAsync(newChef);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        await SendPositionDataWithViewBag();

        var existChef = await _context.Chefs.FindAsync(id);

        if (existChef is null)
        {
            return NotFound();
        }

        ChefUpdateVM vm = new()
        {
            FullName = existChef.FullName,
            Description = existChef.Description,
            PositionId = existChef.PositionId
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ChefUpdateVM vm)
    {
        await SendPositionDataWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existPosition = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);

        if (existPosition is false)
        {
            ModelState.AddModelError("PositionId", "Position is not found!");
            return View(vm);
        }

        var existChef = await _context.Chefs.FindAsync(vm.Id);

        if (existChef is null)
        {
            return NotFound();
        }

        if (!vm.ImagePath?.CheckFileSize(2) ?? false)
        {
            ModelState.AddModelError("ImagePath", "Image size must be less than 2MB!");
            return View(vm);
        }

        if (!vm.ImagePath?.CheckFileType("image") ?? false)
        {
            ModelState.AddModelError("ImagePath", "Image size must be less than 2MB!");
            return View(vm);
        }

        existChef.FullName = vm.FullName;
        existChef.Description = vm.Description;
        existChef.PositionId = vm.PositionId;

        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

        if (vm.ImagePath is { })
        {
            string imageName = vm.ImagePath.SaveFile(folderPath);
            if (System.IO.File.Exists(Path.Combine(folderPath, existChef.ImagePath)))
            {
                System.IO.File.Delete(Path.Combine(folderPath, existChef.ImagePath));
            }
            existChef.ImagePath = imageName;
        }

        _context.Chefs.Update(existChef);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var existChef = await _context.Chefs.FindAsync(id);

        if (existChef is null)
        {
            return NotFound();
        }

        _context.Chefs.Remove(existChef);
        await _context.SaveChangesAsync();

        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");

        if (System.IO.File.Exists(Path.Combine(folderPath, existChef.ImagePath)))
        {
            System.IO.File.Delete(Path.Combine(folderPath, existChef.ImagePath));
        }

        return RedirectToAction("Index");
    }

    private async Task SendPositionDataWithViewBag()
    {
        var positions = await _context.Positions.ToListAsync();
        ViewBag.Positions = positions;
    }
}

using EatrySimulationMPA201.Models;
using EatrySimulationMPA201.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EatrySimulationMPA201.Controllers;

public class AccountController(SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager) : Controller
{
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existUser = await _userManager.FindByNameAsync(vm.UserName);
        if (existUser is { })
        {
            ModelState.AddModelError("UserName", "Username is already exist!");
            return View(vm);
        }

        existUser = await _userManager.FindByEmailAsync(vm.EmailAddress);

        if (existUser is { })
        {
            ModelState.AddModelError("EmailAddress", "Email is already exist!");
            return View(vm);
        }

        AppUser newUser = new()
        {
            FullName = vm.FullName,
            UserName = vm.UserName,
            Email = vm.EmailAddress,
        };

        var result = await _userManager.CreateAsync(newUser, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(" ", error.Description);
            }
            return View(vm);
        }

        await _userManager.AddToRoleAsync(newUser, "Member");
        await _signInManager.SignInAsync(newUser, false);

        return RedirectToAction("Index", "Home");
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await _userManager.FindByEmailAsync(vm.EmailAddress);
        if (user is null)
        {
            ModelState.AddModelError(" ", "Email or password is wrong!");
            return View(vm);
        }

        var checkPassword = await _userManager.CheckPasswordAsync(user, vm.Password);
        if (checkPassword is false)
        {
            ModelState.AddModelError(" ", "Email or password is wrong!");
            return View(vm);
        }

        await _signInManager.SignInAsync(user, vm.IsRemember);

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Admin"
        });

        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Member"
        });

        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Moderator"
        });

        return Ok("Roles created successfully!");
    }
}

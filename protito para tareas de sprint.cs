using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using protipo_sprint_tareas.Models;

namespace protipo_sprint_tareas.Controllers;

// Prototipo original de tu Login. 
// Nota: Este código ya está integrado y funcionando en Controllers/AccountController.cs
public class ProtitoLoginController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public ProtitoLoginController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            false,
            false);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Credenciales incorrectas");
        return View(model);
    }
}
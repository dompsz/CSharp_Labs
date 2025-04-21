using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DependencyInjectionMVC_Auth_Fixed.Controllers;

public class AccountController : Controller
{
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string role, string? returnUrl = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "MyCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("MyCookie", principal);

        return Redirect(returnUrl ?? "/");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookie");
        return RedirectToAction("Login");
    }
}

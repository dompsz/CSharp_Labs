using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DependencyInjectionMVC_Auth_Fixed.Interfaces;

namespace DependencyInjectionMVC_Auth_Fixed.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IMessageService _messageService;

    public HomeController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public IActionResult Index()
    {
        ViewBag.UserName = User.Identity?.Name;
        ViewBag.Role = User.IsInRole("Admin") ? "Admin" : "User";
        ViewBag.Message = _messageService.GetMessage();
        return View();
    }
}

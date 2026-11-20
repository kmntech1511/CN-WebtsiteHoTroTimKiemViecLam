using Microsoft.AspNetCore.Mvc;

namespace WebTimViec.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login() => View();

    [HttpGet]
    public IActionResult Register() => View();
}

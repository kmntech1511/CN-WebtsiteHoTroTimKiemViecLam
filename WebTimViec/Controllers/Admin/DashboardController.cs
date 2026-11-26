using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebTimViec.Controllers.Admin;

[Area("Admin")]
[Authorize(Roles = "QuanTri")]
[Route("Admin/[controller]/[action]")]
public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebTimViec.Data;
using WebTimViec.Models.Account;
using WebTimViec.Models.Entities;
using WebTimViec.Services;

namespace WebTimViec.Controllers;

public class AccountController(AppDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string identifier = model.Identifier.Trim();
        NguoiDung? user = await context.NguoiDungs
            .Include(x => x.MaVaiTroNavigation)
            .FirstOrDefaultAsync(x => x.TenDangNhap == identifier || x.Email == identifier);

        if (user is null || !user.TrangThai || !PasswordService.Verify(model.MatKhau, user.MatKhau))
        {
            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác hoặc tài khoản đã bị khóa.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
            new(ClaimTypes.Name, user.HoTen),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.MaVaiTroNavigation.TenVaiTro)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = model.GhiNhoDangNhap,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(model.GhiNhoDangNhap ? 24 * 30 : 8)
        };

        user.LanDangNhapCuoi = DateTime.Now;
        await context.SaveChangesAsync();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string username = model.TenDangNhap.Trim();
        string email = model.Email.Trim();

        if (await context.NguoiDungs.AnyAsync(x => x.TenDangNhap == username))
        {
            ModelState.AddModelError(nameof(model.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        }

        if (await context.NguoiDungs.AnyAsync(x => x.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng.");
        }

        VaiTro? role = await context.VaiTros.FirstOrDefaultAsync(x => x.TenVaiTro == "NguoiTimViec");
        if (role is null)
        {
            ModelState.AddModelError(string.Empty, "Chưa có vai trò Người tìm việc trong cơ sở dữ liệu.");
        }

        if (!ModelState.IsValid || role is null)
        {
            return View(model);
        }

        context.NguoiDungs.Add(new NguoiDung
        {
            MaVaiTro = role.MaVaiTro,
            TenDangNhap = username,
            MatKhau = PasswordService.Hash(model.MatKhau),
            HoTen = model.HoTen.Trim(),
            Email = email,
            NgayDangKy = DateTime.Now,
            TrangThai = true
        });

        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Đăng ký tài khoản thành công. Bạn có thể đăng nhập ngay.";
        return RedirectToAction(nameof(Login));
    }
}

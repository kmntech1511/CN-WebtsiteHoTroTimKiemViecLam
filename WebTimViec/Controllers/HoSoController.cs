using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTimViec.Data;
using WebTimViec.Models.HoSo;

namespace WebTimViec.Controllers;

[Authorize]
public class HoSoController(AppDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Create() => View(new CreateHoSoViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateHoSoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        bool userExists = await context.NguoiDungs.AnyAsync(x => x.MaNguoiDung == userId);
        if (!userExists)
        {
            return NotFound();
        }

        context.HoSoTimViecs.Add(new Models.Entities.HoSoTimViec
        {
            MaNguoiDung = userId,
            TieuDe = model.TieuDe.Trim(),
            GioiThieuBanThan = model.GioiThieuBanThan?.Trim(),
            MucTieuNgheNghiep = model.MucTieuNgheNghiep?.Trim(),
            HinhThucLamViec = model.HinhThucLamViec?.Trim(),
            MucLuongMongMuon = model.MucLuongMongMuon,
            DiaDiemMongMuon = model.DiaDiemMongMuon?.Trim(),
            NgayTao = DateTime.Now,
            TrangThai = true
        });

        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Tạo hồ sơ tìm việc thành công.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await context.NguoiDungs
            .Include(x => x.HoSoTimViecs)
                .ThenInclude(x => x.CoKyNangs)
            .Include(x => x.HoSoTimViecs)
                .ThenInclude(x => x.KinhNghiemLamViecs)
            .Include(x => x.HoSoTimViecs)
                .ThenInclude(x => x.CoBangCaps)
            .Include(x => x.HoSoTimViecs)
                .ThenInclude(x => x.DatChungChis)
            .FirstOrDefaultAsync(x => x.MaNguoiDung == userId);

        if (user is null)
        {
            return NotFound();
        }

        var model = new HoSoDashboardViewModel
        {
            HoTen = user.HoTen,
            Email = user.Email,
            Sdt = user.Sdt,
            SoHoSo = user.HoSoTimViecs.Count,
            SoKyNang = user.HoSoTimViecs.Sum(x => x.CoKyNangs.Count),
            SoKinhNghiem = user.HoSoTimViecs.Sum(x => x.KinhNghiemLamViecs.Count),
            SoBangCap = user.HoSoTimViecs.Sum(x => x.CoBangCaps.Count),
            SoChungChi = user.HoSoTimViecs.Sum(x => x.DatChungChis.Count),
            HoSos = user.HoSoTimViecs
                .OrderByDescending(x => x.NgayCapNhat ?? x.NgayTao)
                .Select(x => new HoSoSummaryViewModel
                {
                    MaHoSo = x.MaHoSo,
                    TieuDe = x.TieuDe,
                    DiaDiemMongMuon = x.DiaDiemMongMuon,
                    HinhThucLamViec = x.HinhThucLamViec,
                    TrangThai = x.TrangThai,
                    NgayCapNhat = x.NgayCapNhat ?? x.NgayTao
                })
                .ToList()
        };

        return View(model);
    }
}

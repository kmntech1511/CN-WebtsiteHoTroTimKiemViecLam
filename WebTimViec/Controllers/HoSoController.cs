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
    public async Task<IActionResult> Edit(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null)
        {
            return NotFound();
        }

        return View(new CreateHoSoViewModel
        {
            MaHoSo = resume.MaHoSo,
            TieuDe = resume.TieuDe,
            GioiThieuBanThan = resume.GioiThieuBanThan,
            MucTieuNgheNghiep = resume.MucTieuNgheNghiep,
            HinhThucLamViec = resume.HinhThucLamViec,
            MucLuongMongMuon = resume.MucLuongMongMuon,
            DiaDiemMongMuon = resume.DiaDiemMongMuon
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateHoSoViewModel model)
    {
        if (id != model.MaHoSo)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null)
        {
            return NotFound();
        }

        resume.TieuDe = model.TieuDe.Trim();
        resume.GioiThieuBanThan = model.GioiThieuBanThan?.Trim();
        resume.MucTieuNgheNghiep = model.MucTieuNgheNghiep?.Trim();
        resume.HinhThucLamViec = model.HinhThucLamViec?.Trim();
        resume.MucLuongMongMuon = model.MucLuongMongMuon;
        resume.DiaDiemMongMuon = model.DiaDiemMongMuon?.Trim();
        resume.NgayCapNhat = DateTime.Now;

        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công.";
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
                    NgayTao = x.NgayTao,
                    NgayCapNhat = x.NgayCapNhat ?? x.NgayTao
                })
                .ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.MaNguoiDungNavigation)
            .Include(x => x.KinhNghiemLamViecs)
            .Include(x => x.CoKyNangs).ThenInclude(x => x.MaKyNangNavigation)
            .Include(x => x.CoBangCaps).ThenInclude(x => x.MaBangCapNavigation)
            .Include(x => x.DatChungChis).ThenInclude(x => x.MaChungChiNavigation)
            .Include(x => x.MaNganhs)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null) return NotFound();

        var model = new HoSoDetailViewModel
        {
            MaHoSo = resume.MaHoSo,
            HoTen = resume.MaNguoiDungNavigation.HoTen,
            Email = resume.MaNguoiDungNavigation.Email,
            Sdt = resume.MaNguoiDungNavigation.Sdt,
            TieuDe = resume.TieuDe,
            GioiThieuBanThan = resume.GioiThieuBanThan,
            MucTieuNgheNghiep = resume.MucTieuNgheNghiep,
            HinhThucLamViec = resume.HinhThucLamViec,
            MucLuongMongMuon = resume.MucLuongMongMuon,
            DiaDiemMongMuon = resume.DiaDiemMongMuon,
            NgayTao = resume.NgayTao,
            NgayCapNhat = resume.NgayCapNhat,
            KinhNghiems = resume.KinhNghiemLamViecs.OrderByDescending(x => x.TuNgay).Select(x => new ExperienceDetailViewModel
            {
                ChucDanh = x.ChucDanh, TenCongTy = x.TenCongTy, DiaDiem = x.DiaDiemLamViec,
                TuNgay = x.TuNgay, DenNgay = x.DenNgay, MoTa = x.MoTaCongViec
            }).ToList(),
            KyNangs = resume.CoKyNangs.OrderBy(x => x.MaKyNangNavigation.TenKyNang).Select(x => new SkillDetailViewModel
            {
                TenKyNang = x.MaKyNangNavigation.TenKyNang, MucDo = x.MucDo, SoNamSuDung = x.SoNamSuDung
            }).ToList(),
            BangCaps = resume.CoBangCaps.Select(x => new CredentialDetailViewModel
            {
                Ten = x.MaBangCapNavigation.TenBangCap, PhuDe = x.MaBangCapNavigation.CapDo,
                NoiCap = x.TruongDaoTao, ChuyenNganh = x.ChuyenNganh, NamTotNghiep = x.NamTotNghiep
            }).ToList(),
            ChungChis = resume.DatChungChis.Select(x => new CredentialDetailViewModel
            {
                Ten = x.MaChungChiNavigation.TenChungChi, PhuDe = x.MaChungChiNavigation.LoaiChungChi,
                NoiCap = x.NoiCap, NgayCap = x.NgayCap, NgayHetHan = x.NgayHetHan
            }).ToList(),
            NganhNghes = resume.MaNganhs.OrderBy(x => x.TenNganh).Select(x => x.TenNganh).ToList()
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Experiences(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.KinhNghiemLamViecs)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null)
        {
            return NotFound();
        }

        ViewBag.ResumeTitle = resume.TieuDe;
        ViewBag.ResumeId = resume.MaHoSo;
        return View(resume.KinhNghiemLamViecs.OrderByDescending(x => x.TuNgay).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Skills(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.CoKyNangs)
                .ThenInclude(x => x.MaKyNangNavigation)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null)
        {
            return NotFound();
        }

        ViewBag.ResumeTitle = resume.TieuDe;
        ViewBag.ResumeId = resume.MaHoSo;
        var model = resume.CoKyNangs
            .Select(x => new KyNangItemViewModel
            {
                MaKyNang = x.MaKyNang,
                TenKyNang = x.MaKyNangNavigation.TenKyNang,
                LoaiKyNang = x.MaKyNangNavigation.LoaiKyNang,
                MucDo = x.MucDo,
                SoNamSuDung = x.SoNamSuDung
            })
            .OrderBy(x => x.TenKyNang)
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Degrees(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.CoBangCaps)
                .ThenInclude(x => x.MaBangCapNavigation)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null)
        {
            return NotFound();
        }

        ViewBag.ResumeTitle = resume.TieuDe;
        ViewBag.ResumeId = resume.MaHoSo;
        var model = resume.CoBangCaps
            .Select(x => new BangCapItemViewModel
            {
                MaBangCap = x.MaBangCap,
                TenBangCap = x.MaBangCapNavigation.TenBangCap,
                CapDo = x.MaBangCapNavigation.CapDo,
                TruongDaoTao = x.TruongDaoTao,
                ChuyenNganh = x.ChuyenNganh,
                NamTotNghiep = x.NamTotNghiep
            })
            .OrderBy(x => x.TenBangCap)
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Certificates(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.DatChungChis)
                .ThenInclude(x => x.MaChungChiNavigation)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null) return NotFound();

        ViewBag.ResumeTitle = resume.TieuDe;
        ViewBag.ResumeId = resume.MaHoSo;
        var model = resume.DatChungChis
            .Select(x => new ChungChiItemViewModel
            {
                MaChungChi = x.MaChungChi,
                TenChungChi = x.MaChungChiNavigation.TenChungChi,
                LoaiChungChi = x.MaChungChiNavigation.LoaiChungChi,
                SoHieu = x.SoHieu,
                NgayCap = x.NgayCap,
                NgayHetHan = x.NgayHetHan,
                NoiCap = x.NoiCap
            })
            .OrderBy(x => x.TenChungChi)
            .ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AddCertificate(int id)
    {
        if (!await OwnsResume(id)) return NotFound();
        ViewBag.Certificates = await context.ChungChis.Where(x => x.TrangThai).OrderBy(x => x.TenChungChi).ToListAsync();
        return View(new ChungChiViewModel { MaHoSo = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCertificate(ChungChiViewModel model)
    {
        if (!await OwnsResume(model.MaHoSo)) return NotFound();
        if (model.NgayCap.HasValue && model.NgayHetHan.HasValue && model.NgayHetHan < model.NgayCap)
            ModelState.AddModelError(nameof(model.NgayHetHan), "Ngày hết hạn phải sau ngày cấp.");
        if (await context.DatChungChis.AnyAsync(x => x.MaHoSo == model.MaHoSo && x.MaChungChi == model.MaChungChi))
            ModelState.AddModelError(nameof(model.MaChungChi), "Chứng chỉ này đã có trong hồ sơ.");

        if (!ModelState.IsValid)
        {
            ViewBag.Certificates = await context.ChungChis.Where(x => x.TrangThai).OrderBy(x => x.TenChungChi).ToListAsync();
            return View(model);
        }

        context.DatChungChis.Add(new Models.Entities.DatChungChi
        {
            MaHoSo = model.MaHoSo,
            MaChungChi = model.MaChungChi,
            SoHieu = model.SoHieu?.Trim(),
            NgayCap = model.NgayCap,
            NgayHetHan = model.NgayHetHan,
            NoiCap = model.NoiCap?.Trim()
        });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Certificates), new { id = model.MaHoSo });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCertificate(int id, int resumeId)
    {
        if (!await OwnsResume(resumeId)) return NotFound();
        var certificate = await context.DatChungChis.FirstOrDefaultAsync(x => x.MaHoSo == resumeId && x.MaChungChi == id);
        if (certificate is null) return NotFound();
        context.DatChungChis.Remove(certificate);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Certificates), new { id = resumeId });
    }

    [HttpGet]
    public async Task<IActionResult> Industries(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resume = await context.HoSoTimViecs
            .Include(x => x.MaNganhs)
            .FirstOrDefaultAsync(x => x.MaHoSo == id && x.MaNguoiDung == userId);

        if (resume is null) return NotFound();

        var selectedIds = resume.MaNganhs.Select(x => x.MaNganh).ToHashSet();
        var industries = await context.NganhNghes
            .Include(x => x.MaNhomNganhNavigation)
            .Where(x => x.TrangThai)
            .OrderBy(x => x.MaNhomNganhNavigation.TenNhomNganh)
            .ThenBy(x => x.TenNganh)
            .ToListAsync();

        var model = new NganhNgheViewModel
        {
            MaHoSo = id,
            MaNganhDaChon = selectedIds.ToList(),
            NganhNghes = industries.Select(x => new NganhNgheItemViewModel
            {
                MaNganh = x.MaNganh,
                TenNganh = x.TenNganh,
                TenNhomNganh = x.MaNhomNganhNavigation.TenNhomNganh,
                DaChon = selectedIds.Contains(x.MaNganh)
            }).ToList()
        };

        ViewBag.ResumeTitle = resume.TieuDe;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Industries(NganhNgheViewModel model)
    {
        if (!await OwnsResume(model.MaHoSo)) return NotFound();

        var resume = await context.HoSoTimViecs
            .Include(x => x.MaNganhs)
            .FirstAsync(x => x.MaHoSo == model.MaHoSo);
        var validIds = await context.NganhNghes
            .Where(x => x.TrangThai && model.MaNganhDaChon.Contains(x.MaNganh))
            .Select(x => x.MaNganh)
            .ToListAsync();

        resume.MaNganhs.Clear();
        var selectedIndustries = await context.NganhNghes
            .Where(x => validIds.Contains(x.MaNganh))
            .ToListAsync();
        foreach (var industry in selectedIndustries)
        {
            resume.MaNganhs.Add(industry);
        }

        resume.NgayCapNhat = DateTime.Now;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Industries), new { id = model.MaHoSo });
    }

    [HttpGet]
    public async Task<IActionResult> AddDegree(int id)
    {
        if (!await OwnsResume(id)) return NotFound();
        ViewBag.Degrees = await context.BangCaps.Where(x => x.TrangThai).OrderBy(x => x.TenBangCap).ToListAsync();
        return View(new BangCapViewModel { MaHoSo = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDegree(BangCapViewModel model)
    {
        if (!await OwnsResume(model.MaHoSo)) return NotFound();
        if (await context.CoBangCaps.AnyAsync(x => x.MaHoSo == model.MaHoSo && x.MaBangCap == model.MaBangCap))
            ModelState.AddModelError(nameof(model.MaBangCap), "Bằng cấp này đã có trong hồ sơ.");

        if (!ModelState.IsValid)
        {
            ViewBag.Degrees = await context.BangCaps.Where(x => x.TrangThai).OrderBy(x => x.TenBangCap).ToListAsync();
            return View(model);
        }

        context.CoBangCaps.Add(new Models.Entities.CoBangCap
        {
            MaHoSo = model.MaHoSo,
            MaBangCap = model.MaBangCap,
            TruongDaoTao = model.TruongDaoTao?.Trim(),
            ChuyenNganh = model.ChuyenNganh?.Trim(),
            NamTotNghiep = model.NamTotNghiep
        });
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Degrees), new { id = model.MaHoSo });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDegree(int id, int resumeId)
    {
        if (!await OwnsResume(resumeId)) return NotFound();
        var degree = await context.CoBangCaps.FirstOrDefaultAsync(x => x.MaHoSo == resumeId && x.MaBangCap == id);
        if (degree is null) return NotFound();
        context.CoBangCaps.Remove(degree);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Degrees), new { id = resumeId });
    }

    [HttpGet]
    public async Task<IActionResult> AddSkill(int id)
    {
        if (!await OwnsResume(id))
        {
            return NotFound();
        }

        ViewBag.Skills = await context.KyNangs
            .Where(x => x.TrangThai)
            .OrderBy(x => x.TenKyNang)
            .ToListAsync();
        return View(new KyNangViewModel { MaHoSo = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSkill(KyNangViewModel model)
    {
        if (!await OwnsResume(model.MaHoSo))
        {
            return NotFound();
        }

        bool alreadyAdded = await context.CoKyNangs
            .AnyAsync(x => x.MaHoSo == model.MaHoSo && x.MaKyNang == model.MaKyNang);
        if (alreadyAdded)
        {
            ModelState.AddModelError(nameof(model.MaKyNang), "Kỹ năng này đã có trong hồ sơ.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Skills = await context.KyNangs
                .Where(x => x.TrangThai)
                .OrderBy(x => x.TenKyNang)
                .ToListAsync();
            return View(model);
        }

        context.CoKyNangs.Add(new Models.Entities.CoKyNang
        {
            MaHoSo = model.MaHoSo,
            MaKyNang = model.MaKyNang,
            MucDo = model.MucDo,
            SoNamSuDung = model.SoNamSuDung
        });

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Skills), new { id = model.MaHoSo });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSkill(int id, int resumeId)
    {
        if (!await OwnsResume(resumeId))
        {
            return NotFound();
        }

        var skill = await context.CoKyNangs
            .FirstOrDefaultAsync(x => x.MaHoSo == resumeId && x.MaKyNang == id);
        if (skill is null)
        {
            return NotFound();
        }

        context.CoKyNangs.Remove(skill);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Skills), new { id = resumeId });
    }

    [HttpGet]
    public async Task<IActionResult> AddExperience(int id)
    {
        if (!await OwnsResume(id))
        {
            return NotFound();
        }

        return View(new KinhNghiemViewModel { MaHoSo = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddExperience(KinhNghiemViewModel model)
    {
        if (model.DenNgay.HasValue && model.DenNgay < model.TuNgay)
        {
            ModelState.AddModelError(nameof(model.DenNgay), "Ngày kết thúc phải sau ngày bắt đầu.");
        }

        if (!ModelState.IsValid || !await OwnsResume(model.MaHoSo))
        {
            return View(model);
        }

        context.KinhNghiemLamViecs.Add(new Models.Entities.KinhNghiemLamViec
        {
            MaHoSo = model.MaHoSo,
            TenCongTy = model.TenCongTy.Trim(),
            ChucDanh = model.ChucDanh.Trim(),
            DiaDiemLamViec = model.DiaDiemLamViec?.Trim(),
            TuNgay = model.TuNgay,
            DenNgay = model.DenNgay,
            MoTaCongViec = model.MoTaCongViec?.Trim()
        });

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Experiences), new { id = model.MaHoSo });
    }

    [HttpGet]
    public async Task<IActionResult> EditExperience(int id)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var experience = await context.KinhNghiemLamViecs
            .FirstOrDefaultAsync(x => x.MaKinhNghiem == id && x.MaHoSoNavigation.MaNguoiDung == userId);

        if (experience is null)
        {
            return NotFound();
        }

        return View(new KinhNghiemViewModel
        {
            MaKinhNghiem = experience.MaKinhNghiem,
            MaHoSo = experience.MaHoSo,
            TenCongTy = experience.TenCongTy,
            ChucDanh = experience.ChucDanh,
            DiaDiemLamViec = experience.DiaDiemLamViec,
            TuNgay = experience.TuNgay,
            DenNgay = experience.DenNgay,
            MoTaCongViec = experience.MoTaCongViec
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditExperience(KinhNghiemViewModel model)
    {
        if (model.DenNgay.HasValue && model.DenNgay < model.TuNgay)
        {
            ModelState.AddModelError(nameof(model.DenNgay), "Ngày kết thúc phải sau ngày bắt đầu.");
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var experience = await context.KinhNghiemLamViecs
            .FirstOrDefaultAsync(x => x.MaKinhNghiem == model.MaKinhNghiem && x.MaHoSoNavigation.MaNguoiDung == userId);

        if (experience is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        experience.TenCongTy = model.TenCongTy.Trim();
        experience.ChucDanh = model.ChucDanh.Trim();
        experience.DiaDiemLamViec = model.DiaDiemLamViec?.Trim();
        experience.TuNgay = model.TuNgay;
        experience.DenNgay = model.DenNgay;
        experience.MoTaCongViec = model.MoTaCongViec?.Trim();
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Experiences), new { id = experience.MaHoSo });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteExperience(int id, int resumeId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var experience = await context.KinhNghiemLamViecs
            .FirstOrDefaultAsync(x => x.MaKinhNghiem == id && x.MaHoSo == resumeId && x.MaHoSoNavigation.MaNguoiDung == userId);

        if (experience is null)
        {
            return NotFound();
        }

        context.KinhNghiemLamViecs.Remove(experience);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Experiences), new { id = resumeId });
    }

    private async Task<bool> OwnsResume(int resumeId)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return await context.HoSoTimViecs.AnyAsync(x => x.MaHoSo == resumeId && x.MaNguoiDung == userId);
    }
}

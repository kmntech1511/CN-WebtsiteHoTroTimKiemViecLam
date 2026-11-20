using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public int MaVaiTro { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Sdt { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? AnhDaiDienSmall { get; set; }

    public string? AnhDaiDienMedium { get; set; }

    public string? AnhDaiDienLarge { get; set; }

    public DateTime NgayDangKy { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<HoSoTimViec> HoSoTimViecs { get; set; } = new List<HoSoTimViec>();

    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
}

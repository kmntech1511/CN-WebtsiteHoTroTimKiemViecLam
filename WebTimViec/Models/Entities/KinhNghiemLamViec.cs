using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class KinhNghiemLamViec
{
    public int MaKinhNghiem { get; set; }

    public int MaHoSo { get; set; }

    public string TenCongTy { get; set; } = null!;

    public string ChucDanh { get; set; } = null!;

    public string? DiaDiemLamViec { get; set; }

    public DateOnly TuNgay { get; set; }

    public DateOnly? DenNgay { get; set; }

    public string? MoTaCongViec { get; set; }

    public virtual HoSoTimViec MaHoSoNavigation { get; set; } = null!;
}

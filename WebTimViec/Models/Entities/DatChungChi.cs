using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class DatChungChi
{
    public int MaHoSo { get; set; }

    public int MaChungChi { get; set; }

    public string? SoHieu { get; set; }

    public DateOnly? NgayCap { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public string? NoiCap { get; set; }

    public virtual ChungChi MaChungChiNavigation { get; set; } = null!;

    public virtual HoSoTimViec MaHoSoNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class CoKyNang
{
    public int MaHoSo { get; set; }

    public int MaKyNang { get; set; }

    public string? MucDo { get; set; }

    public decimal? SoNamSuDung { get; set; }

    public virtual HoSoTimViec MaHoSoNavigation { get; set; } = null!;

    public virtual KyNang MaKyNangNavigation { get; set; } = null!;
}

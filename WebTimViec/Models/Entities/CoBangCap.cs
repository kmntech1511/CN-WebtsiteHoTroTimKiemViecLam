using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class CoBangCap
{
    public int MaHoSo { get; set; }

    public int MaBangCap { get; set; }

    public string? TruongDaoTao { get; set; }

    public string? ChuyenNganh { get; set; }

    public int? NamTotNghiep { get; set; }

    public virtual BangCap MaBangCapNavigation { get; set; } = null!;

    public virtual HoSoTimViec MaHoSoNavigation { get; set; } = null!;
}

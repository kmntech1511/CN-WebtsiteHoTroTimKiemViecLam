using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class NganhNghe
{
    public int MaNganh { get; set; }

    public int MaNhomNganh { get; set; }

    public string TenNganh { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? HinhAnhSmall { get; set; }

    public string? HinhAnhMedium { get; set; }

    public string? HinhAnhLarge { get; set; }

    public bool TrangThai { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual NhomNganh MaNhomNganhNavigation { get; set; } = null!;

    public virtual ICollection<HoSoTimViec> MaHoSos { get; set; } = new List<HoSoTimViec>();
}

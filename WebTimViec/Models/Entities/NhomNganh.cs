using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class NhomNganh
{
    public int MaNhomNganh { get; set; }

    public string TenNhomNganh { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? HinhAnhSmall { get; set; }

    public string? HinhAnhMedium { get; set; }

    public string? HinhAnhLarge { get; set; }

    public int ThuTuHienThi { get; set; }

    public bool TrangThai { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<NganhNghe> NganhNghes { get; set; } = new List<NganhNghe>();
}

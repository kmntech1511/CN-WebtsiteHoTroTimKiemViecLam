using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class KyNang
{
    public int MaKyNang { get; set; }

    public string TenKyNang { get; set; } = null!;

    public string? LoaiKyNang { get; set; }

    public string? MoTa { get; set; }

    public bool TrangThai { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<CoKyNang> CoKyNangs { get; set; } = new List<CoKyNang>();
}

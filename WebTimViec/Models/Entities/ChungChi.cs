using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class ChungChi
{
    public int MaChungChi { get; set; }

    public string TenChungChi { get; set; } = null!;

    public string? LoaiChungChi { get; set; }

    public string? MoTa { get; set; }

    public bool TrangThai { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<DatChungChi> DatChungChis { get; set; } = new List<DatChungChi>();
}

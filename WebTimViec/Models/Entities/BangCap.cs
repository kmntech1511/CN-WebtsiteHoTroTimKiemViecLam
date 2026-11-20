using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class BangCap
{
    public int MaBangCap { get; set; }

    public string TenBangCap { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? CapDo { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<CoBangCap> CoBangCaps { get; set; } = new List<CoBangCap>();
}

using System;
using System.Collections.Generic;

namespace WebTimViec.Models.Entities;

public partial class HoSoTimViec
{
    public int MaHoSo { get; set; }

    public int MaNguoiDung { get; set; }

    public string TieuDe { get; set; } = null!;

    public string? GioiThieuBanThan { get; set; }

    public string? MucTieuNgheNghiep { get; set; }

    public string? HinhThucLamViec { get; set; }

    public decimal? MucLuongMongMuon { get; set; }

    public string? DiaDiemMongMuon { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<CoBangCap> CoBangCaps { get; set; } = new List<CoBangCap>();

    public virtual ICollection<CoKyNang> CoKyNangs { get; set; } = new List<CoKyNang>();

    public virtual ICollection<DatChungChi> DatChungChis { get; set; } = new List<DatChungChi>();

    public virtual ICollection<KinhNghiemLamViec> KinhNghiemLamViecs { get; set; } = new List<KinhNghiemLamViec>();

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<NganhNghe> MaNganhs { get; set; } = new List<NganhNghe>();
}

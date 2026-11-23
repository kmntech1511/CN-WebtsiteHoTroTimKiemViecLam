namespace WebTimViec.Models.HoSo;

public class HoSoDashboardViewModel
{
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public int SoHoSo { get; set; }
    public int SoKyNang { get; set; }
    public int SoKinhNghiem { get; set; }
    public int SoBangCap { get; set; }
    public int SoChungChi { get; set; }
    public List<HoSoSummaryViewModel> HoSos { get; set; } = [];
}

public class HoSoSummaryViewModel
{
    public int MaHoSo { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string? DiaDiemMongMuon { get; set; }
    public string? HinhThucLamViec { get; set; }
    public bool TrangThai { get; set; }
    public DateTime NgayCapNhat { get; set; }
}

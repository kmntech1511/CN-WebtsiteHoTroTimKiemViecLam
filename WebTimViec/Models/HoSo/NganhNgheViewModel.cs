namespace WebTimViec.Models.HoSo;

public class NganhNgheViewModel
{
    public int MaHoSo { get; set; }
    public List<int> MaNganhDaChon { get; set; } = [];
    public List<NganhNgheItemViewModel> NganhNghes { get; set; } = [];
}

public class NganhNgheItemViewModel
{
    public int MaNganh { get; set; }
    public string TenNganh { get; set; } = string.Empty;
    public string? TenNhomNganh { get; set; }
    public bool DaChon { get; set; }
}

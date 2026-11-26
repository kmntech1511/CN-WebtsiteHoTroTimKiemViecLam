namespace WebTimViec.Models.HoSo;

public class HoSoDetailViewModel
{
    public int MaHoSo { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Sdt { get; set; }
    public string TieuDe { get; set; } = string.Empty;
    public string? GioiThieuBanThan { get; set; }
    public string? MucTieuNgheNghiep { get; set; }
    public string? HinhThucLamViec { get; set; }
    public decimal? MucLuongMongMuon { get; set; }
    public string? DiaDiemMongMuon { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime? NgayCapNhat { get; set; }
    public List<ExperienceDetailViewModel> KinhNghiems { get; set; } = [];
    public List<SkillDetailViewModel> KyNangs { get; set; } = [];
    public List<CredentialDetailViewModel> BangCaps { get; set; } = [];
    public List<CredentialDetailViewModel> ChungChis { get; set; } = [];
    public List<string> NganhNghes { get; set; } = [];
}

public class ExperienceDetailViewModel
{
    public string ChucDanh { get; set; } = string.Empty;
    public string TenCongTy { get; set; } = string.Empty;
    public string? DiaDiem { get; set; }
    public DateOnly TuNgay { get; set; }
    public DateOnly? DenNgay { get; set; }
    public string? MoTa { get; set; }
}

public class SkillDetailViewModel
{
    public string TenKyNang { get; set; } = string.Empty;
    public string? MucDo { get; set; }
    public decimal? SoNamSuDung { get; set; }
}

public class CredentialDetailViewModel
{
    public string Ten { get; set; } = string.Empty;
    public string? PhuDe { get; set; }
    public string? NoiCap { get; set; }
    public string? ChuyenNganh { get; set; }
    public int? NamTotNghiep { get; set; }
    public DateOnly? NgayCap { get; set; }
    public DateOnly? NgayHetHan { get; set; }
}

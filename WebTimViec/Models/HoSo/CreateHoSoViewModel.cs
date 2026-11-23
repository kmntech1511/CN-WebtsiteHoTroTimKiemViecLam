using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.HoSo;

public class CreateHoSoViewModel
{
    public int MaHoSo { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề hồ sơ.")]
    [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
    [Display(Name = "Tiêu đề hồ sơ")]
    public string TieuDe { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Giới thiệu bản thân")]
    public string? GioiThieuBanThan { get; set; }

    [StringLength(500)]
    [Display(Name = "Mục tiêu nghề nghiệp")]
    public string? MucTieuNgheNghiep { get; set; }

    [StringLength(50)]
    [Display(Name = "Hình thức làm việc")]
    public string? HinhThucLamViec { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Mức lương không được âm.")]
    [Display(Name = "Mức lương mong muốn")]
    public decimal? MucLuongMongMuon { get; set; }

    [StringLength(200)]
    [Display(Name = "Địa điểm mong muốn")]
    public string? DiaDiemMongMuon { get; set; }
}

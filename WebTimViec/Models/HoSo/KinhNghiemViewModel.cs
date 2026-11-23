using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.HoSo;

public class KinhNghiemViewModel
{
    public int MaKinhNghiem { get; set; }
    public int MaHoSo { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên công ty.")]
    [StringLength(200)]
    [Display(Name = "Tên công ty")]
    public string TenCongTy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập chức danh.")]
    [StringLength(150)]
    [Display(Name = "Chức danh")]
    public string ChucDanh { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Địa điểm làm việc")]
    public string? DiaDiemLamViec { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu.")]
    [Display(Name = "Từ ngày")]
    public DateOnly TuNgay { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Đến ngày")]
    public DateOnly? DenNgay { get; set; }

    [StringLength(1000)]
    [Display(Name = "Mô tả công việc")]
    public string? MoTaCongViec { get; set; }
}

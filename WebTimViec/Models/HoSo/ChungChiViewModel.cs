using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.HoSo;

public class ChungChiViewModel
{
    public int MaHoSo { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn chứng chỉ.")]
    public int MaChungChi { get; set; }

    [StringLength(100)]
    [Display(Name = "Số hiệu")]
    public string? SoHieu { get; set; }

    [Display(Name = "Ngày cấp")]
    public DateOnly? NgayCap { get; set; }

    [Display(Name = "Ngày hết hạn")]
    public DateOnly? NgayHetHan { get; set; }

    [StringLength(200)]
    [Display(Name = "Nơi cấp")]
    public string? NoiCap { get; set; }
}

public class ChungChiItemViewModel
{
    public int MaChungChi { get; set; }
    public string TenChungChi { get; set; } = string.Empty;
    public string? LoaiChungChi { get; set; }
    public string? SoHieu { get; set; }
    public DateOnly? NgayCap { get; set; }
    public DateOnly? NgayHetHan { get; set; }
    public string? NoiCap { get; set; }
}

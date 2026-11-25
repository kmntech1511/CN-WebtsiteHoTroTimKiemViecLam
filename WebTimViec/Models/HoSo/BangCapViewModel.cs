using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.HoSo;

public class BangCapViewModel
{
    public int MaHoSo { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn bằng cấp.")]
    public int MaBangCap { get; set; }

    [StringLength(200)]
    [Display(Name = "Trường đào tạo")]
    public string? TruongDaoTao { get; set; }

    [StringLength(150)]
    [Display(Name = "Chuyên ngành")]
    public string? ChuyenNganh { get; set; }

    [Range(1950, 2100, ErrorMessage = "Năm tốt nghiệp không hợp lệ.")]
    [Display(Name = "Năm tốt nghiệp")]
    public int? NamTotNghiep { get; set; }
}

public class BangCapItemViewModel
{
    public int MaBangCap { get; set; }
    public string TenBangCap { get; set; } = string.Empty;
    public string? CapDo { get; set; }
    public string? TruongDaoTao { get; set; }
    public string? ChuyenNganh { get; set; }
    public int? NamTotNghiep { get; set; }
}

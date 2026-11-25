using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.HoSo;

public class KyNangViewModel
{
    public int MaHoSo { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn kỹ năng.")]
    public int MaKyNang { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn mức độ.")]
    [Display(Name = "Mức độ")]
    public string MucDo { get; set; } = string.Empty;

    [Range(0, 80, ErrorMessage = "Số năm sử dụng phải từ 0 đến 80.")]
    [Display(Name = "Số năm sử dụng")]
    public decimal? SoNamSuDung { get; set; }
}

public class KyNangItemViewModel
{
    public int MaKyNang { get; set; }
    public string TenKyNang { get; set; } = string.Empty;
    public string? LoaiKyNang { get; set; }
    public string? MucDo { get; set; }
    public decimal? SoNamSuDung { get; set; }
}

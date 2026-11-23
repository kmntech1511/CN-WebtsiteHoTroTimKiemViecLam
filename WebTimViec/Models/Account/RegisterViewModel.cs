using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.Account;

public class RegisterViewModel
{
    [Display(Name = "Họ và tên")]
    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3 đến 50 ký tự.")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ email")]
    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    [Display(Name = "Xác nhận mật khẩu")]
    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
    [DataType(DataType.Password)]
    [Compare(nameof(MatKhau), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    public string XacNhanMatKhau { get; set; } = string.Empty;

    [Display(Name = "Điều khoản sử dụng")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn cần đồng ý với điều khoản sử dụng.")]
    public bool DongYDieuKhoan { get; set; }
}

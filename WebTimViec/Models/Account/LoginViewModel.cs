using System.ComponentModel.DataAnnotations;

namespace WebTimViec.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email hoặc tên đăng nhập.")]
    [Display(Name = "Email hoặc tên đăng nhập")]
    public string Identifier { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    public bool GhiNhoDangNhap { get; set; }
}

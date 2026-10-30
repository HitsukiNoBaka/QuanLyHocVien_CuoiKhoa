using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHocVien.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập hoặc ID.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        public string UID { get; set; } = string.Empty;

    }
}

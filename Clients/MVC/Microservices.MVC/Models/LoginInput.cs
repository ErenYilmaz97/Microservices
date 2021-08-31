using System.ComponentModel.DataAnnotations;

namespace Microservices.MVC.Models
{
    public class LoginInput
    {
        [Display(Name = "Email Adresi")]
        [Required(ErrorMessage = "Email Boş Olamaz.")]
        public string Email { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre Boş Olamaz.")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
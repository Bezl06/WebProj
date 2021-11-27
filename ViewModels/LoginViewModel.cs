using System.ComponentModel.DataAnnotations;

namespace MvcFrilance.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^\w+$")]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
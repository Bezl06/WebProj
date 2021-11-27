using System.ComponentModel.DataAnnotations;

namespace MvcFrilance.ViewModels
{
    public class RegisterViewModel
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
        [Required]
        [StringLength(30, MinimumLength = 4)]
        [RegularExpression(@"^\w+$")]
        [Display(Name = "Никнэйм")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        public bool IsFrelancer { get; set; }
    }
}
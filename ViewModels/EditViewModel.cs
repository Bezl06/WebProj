using System.ComponentModel.DataAnnotations;
using MvcFrilance.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcFrilance.ViewModels
{
    public class EditClientViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(18, MinimumLength = 6)]
        [RegularExpression(@"^[\w-._@+]+$")]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        [RegularExpression(@"^[\w-._@+]+$")]
        [Display(Name = "Имя пользовеля (или псевдоним)")]
        public string NickName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
    }
    public class EditFrilancerModel : EditClientViewModel
    {
        [Display(Name = "Ваше описание")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<string> Spells { get; set; } = new();
        public IEnumerable<Tag> DefaultTags { get; set; }
        public IEnumerable<Spell> AllSpells { get; set; }
    }
}
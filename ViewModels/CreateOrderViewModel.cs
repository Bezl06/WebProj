using System.ComponentModel.DataAnnotations;

namespace MvcFrilance.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "Введите валидную сумму")]
        [Display(Name = "Бюджет")]
        public decimal Price { get; set; }
        [Required]
        public string TypePrice { get; set; }
        public string OrderType { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcFrilance.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Сфера деятельности")]
        public string OrderType { get; set; }
        public DateTime CreateDate { get; set; }
        public int Views { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Бюджет")]
        public decimal Price { get; set; }
        [Required]
        public string TypePrice { get; set; }

        public List<Tag> Tags { get; set; }


        public string UserId { get; set; }
        public User User { get; set; }
    }
}
using ShopApp.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(20,MinimumLength =1,ErrorMessage ="Ürün İsmi Minumum 1 ve maximum 20 karakter olmalıdır.")]
        public string Name { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 5, ErrorMessage = " Ürün Açıklaması  Minumum 5 ve maximum 100 karakter olmalıdır.")]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage ="Fiyat Belirtiniz")]
        [Range(1,10000)]  //1 ie 10000 arası değer alır
        public decimal? Price { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}

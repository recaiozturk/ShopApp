using System.Collections.Generic;
using System.Linq;

namespace ShopApp.WebUI.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public List<CartItemModel> CartItems { get; set; }

        public decimal TotalPrice()
        {
            return CartItems.Sum(i=>i.Price*i.Quantity);
        }
    }

    public class CartItemModel
    {
        public int  CartItemId { get; set; }
        public int  ProductId { get; set; }
        public string  Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}

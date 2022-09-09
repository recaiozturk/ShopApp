using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {

        private ICartDal _cartDal;

        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;

        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                //daha önce o productdan var mı kontrol ediyoruz
                var index=cart.CartItems.FindIndex(i=>i.ProductId==productId);

                if (index < 0)
                {
                    cart.CartItems.Add(new CartItem()
                    {
                        ProductId=productId,
                        Quantity=quantity,
                        CartId=cart.Id
                    });
                }
                else
                {
                    //daha önceki product sepete eklenmiş ise önceki ile şuanki isteden miktarı topluyoruz
                    cart.CartItems[index].Quantity += quantity;
                }

                _cartDal.Update(cart);  
                //burada normal update kullanamıyoruz çünkü cart modelindeki ilişkili nesneleri(CartItems) güncelleyemiyor
                //bunun için EfCoreCartdal da Update methodunu virtual olarak tanımlayıp ovveride edilebilmesini sağlıyacaz
            }
        }

        public void ClearCart(int cartId)
        {
            _cartDal.ClearCart(cartId);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                _cartDal.DeleteFromCart(cart.Id, productId);
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            return _cartDal.GetByUserId(userId);
        }

        //Kart Oluşturma
        public void InitializeCart(string userId)
        {
            _cartDal.Create(new Cart() { UserId = userId });
        }
    }
}

using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {

        private ICartService _cartService;
        private IOrderService _orderService;
        private UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService,IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
            return RedirectToAction("Index");
        }


        public IActionResult CheckOut()
        {

            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (decimal)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public IActionResult CheckOut(OrderModel model)
        {


            if (ModelState.IsValid)
            {

                var userId=_userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel() {

                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity

                    }).ToList()
                };

                //Ödeme
                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {

                    SaveOrder(model, payment, userId);
                    //ClearCart(userId);

                    return View("Success");
                }
                else
                {
                    //Hata Göster
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Ödeme Hata",
                        Message = payment.ErrorMessage,
                        Css = "danger"
                    });


                    return View(model);
                }


                //sipariş
            }

            return View(model);
        }

        private void ClearCart(string userId)
        {
            throw new System.NotImplementedException();
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111,999999).ToString();
            order.OrderState = EnumOrderState.Complate;
            order.PaymentTypes = EnumPaymentTypes.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName=model.FirstName;
            order.LastName=model.LastName;
            order.Email=model.Email;
            order.Phone=model.Phone;
            order.City=model.City;
            order.Adress = model.Address;
            order.UserId=userId;

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                };
                order.OrderItems.Add(orderItem);
            }

            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "your api key";
            options.SecretKey = "your secret key";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";





            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            //request.ConversationId = "123456789";
            request.ConversationId=Guid.NewGuid().ToString();   //benzersiz oluşturur
            request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0];  //Total Price - Toplam
            request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0];  // Vergiler Dahil
            request.Currency = Currency.TRY.ToString();  //Para Birimi
            request.Installment = 1;  //Taksit Sayısı
            //request.BasketId = "B67832";
            request.BasketId = model.CartModel.CartId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            //Kredi kart Bilgiler   
            PaymentCard paymentCard = new PaymentCard();


            //paymentCard.CardHolderName = "John Doe";
            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";
            //paymentCard.RegisterCard = 0;

            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;


            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Phone";  //Buraya Kategori Bilgisini tasıyacaz
                //basketItem.Category2 = "Accessories";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = (item.Quantity*item.Price).ToString().Split(",")[0];  //Gecici çözüm

                basketItems.Add(basketItem);

            }

            request.BasketItems = basketItems;

            //BasketItem firstBasketItem = new BasketItem();
            //firstBasketItem.Id = "BI101";
            //firstBasketItem.Name = "Binocular";
            //firstBasketItem.Category1 = "Collectibles";
            //firstBasketItem.Category2 = "Accessories";
            //firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            //firstBasketItem.Price = "0.3";
            //basketItems.Add(firstBasketItem);

            //BasketItem secondBasketItem = new BasketItem();
            //secondBasketItem.Id = "BI102";
            //secondBasketItem.Name = "Game code";
            //secondBasketItem.Category1 = "Game";
            //secondBasketItem.Category2 = "Online Game Items";
            //secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            //secondBasketItem.Price = "0.5";
            //basketItems.Add(secondBasketItem);

            //BasketItem thirdBasketItem = new BasketItem();
            //thirdBasketItem.Id = "BI103";
            //thirdBasketItem.Name = "Usb";
            //thirdBasketItem.Category1 = "Electronics";
            //thirdBasketItem.Category2 = "Usb / Cable";
            //thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            //thirdBasketItem.Price = "0.2";
            //basketItems.Add(thirdBasketItem);
            //request.BasketItems = basketItems;

            return Payment.Create(request, options);

            
        }

    }
}

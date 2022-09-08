using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }

        public EnumOrderState OrderState { get; set; }

        public EnumPaymentTypes PaymentTypes { get; set; }

        public string LastName { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OrderNote { get; set; }
        public int MyProperty { get; set; }

        //Sipariş Satırları diyebiliriz
        public List<OrderItem> OrderItems { get; set; }

        //Payment API 
        public string PaymentId { get; set; }
        public string PaymentToken { get; set; }
        public string  ConversationId { get; set; }



    }

    public enum EnumPaymentTypes
    {
        CreditCard=0,
        Eft=1
        //vs
    }

    public enum EnumOrderState
    {
        //beklemede
        Waiting=0,

        //ödenmemiş
        Unpaid=1,

        //tamamlanmış
        Complate=2,

        //kargo Süreci
        Cargo=3



    }

}

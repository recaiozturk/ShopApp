using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order, ShopContext>, IOrderDal
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new ShopContext())
            {
                var orders=context.Orders
                    .Include(i=>i.OrderItems)
                    .ThenInclude(i=>i.Product)
                    .AsQueryable();   // bu aşamada veri tabanına sorgu gitmez,bunun üzerinde işlem yapıcaz

                if (!string.IsNullOrEmpty(userId))   //gelen userId null değil ise
                {
                    orders=orders.Where(i=>i.UserId == userId);
                }

                return orders.ToList(); // burda veri tabanına sorgu gider
            }
        }
    }
}

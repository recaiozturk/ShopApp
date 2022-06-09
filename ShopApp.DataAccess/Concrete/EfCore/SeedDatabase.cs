using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            //bekleyen migrations var mı kontrol ediyoruz
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if(context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                }

                context.SaveChanges();
            }
        }


        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},
            new Category(){Name="Bilgisayar"}
        };

        private static Product[] Products =
        {
            new Product(){Name="Samsung S4",Price=1000,ImageUrl="1.jpg"},
            new Product(){Name="Samsung S5",Price=2000,ImageUrl="1.jpg"},
            new Product(){Name="Samsung S6",Price=3000,ImageUrl="1.jpg"},
            new Product(){Name="Samsung S7",Price=4000,ImageUrl="1.jpg"},
            new Product(){Name="IPhone 6S",Price=31000,ImageUrl="1.jpg"},
            new Product(){Name="IPhone 7S",Price=12000,ImageUrl="1.jpg"},
            new Product(){Name="IPhone 8S",Price=14000,ImageUrl="1.jpg"},

        };
    }
}

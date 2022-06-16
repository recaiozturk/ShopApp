using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;

using ShopApp.WebUI.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //services.AddScoped<IProductDal, MemoryProductDal>();
            services.AddScoped<IProductDal, EfCoreProductDal>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IcategoryServicee, CategoryManager>();



            services.AddControllersWithViews(); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            //app.CustomStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMvcWithDefaultRoute();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(

                    name: "adminProducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "Index" }

                    );

                endpoints.MapControllerRoute(

                    name: "adminProducts",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "Edit" }

                    );

                endpoints.MapControllerRoute(

                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new { controller ="Shop",action="List"}

                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"

                    );

            });
        }
    }
}

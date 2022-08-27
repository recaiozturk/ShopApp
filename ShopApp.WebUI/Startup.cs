using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
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

            //Identity db için ekleme yapýyoruz
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //passwordun rakam içermesi için
                options.Password.RequireDigit = true;

                //passwordun kucuk karakter içermesi için
                options.Password.RequireLowercase=true;

                //minumum kac karakter olacaðý
                options.Password.RequiredLength = 6;

                //AlphaNumeric girme zorunluluðu olmasýn
                options.Password.RequireNonAlphanumeric = true;

                //mutlaka buyuk karakter icersin
                options.Password.RequireUppercase=true;

                //Kullanýcýnýn kac kere yanlýs parola girme hakký olsun
                options.Lockout.MaxFailedAccessAttempts = 5;

                //failed olduktan sonra login olmasý için gereken süre
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                //Lockout sistemi yeni kullanýcý için de geçerli olsun
                options.Lockout.AllowedForNewUsers = true;

                //tek email ile uyelik oluþturma
                options.User.RequireUniqueEmail=true;

                //mail ve telefon onaylama
                options.SignIn.RequireConfirmedEmail=true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //sessiondaki cookie ayarlarý
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accesdenied";

                //1 Gün boyunca cookiler tarayýcýda saklanýr.    
                //options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Varsayýlan deðer 20 dakkadýr

                //uygulama kullanýlýrken cookie kapanma suresi- false olursa uykulama kulanýlsýn ya da kullanýlmasý n20 dakka sonra sona erer
                options.SlidingExpiration = true;

                options.Cookie = new CookieBuilder
                {
                    //scriptler cookileri okuyabilir
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie"
                };
            });

            //services.AddScoped<IProductDal, MemoryProductDal>();
            services.AddScoped<IProductDal, EfCoreProductDal>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IcategoryServicee, CategoryManager>();

            services.AddTransient<IEmailSender, EmailSender>();



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

            app.UseAuthentication(); 
            app.UseAuthorization();

            //app.UseMvcWithDefaultRoute();



            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(

                    name: "adminProducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", action = "ProductList" }

                    );

                endpoints.MapControllerRoute(

                    name: "adminProducts",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "EditProduct" }

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

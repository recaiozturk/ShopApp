using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
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

            //Identity db i�in ekleme yap�yoruz
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //passwordun rakam i�ermesi i�in
                options.Password.RequireDigit = true;

                //passwordun kucuk karakter i�ermesi i�in
                options.Password.RequireLowercase=true;

                //minumum kac karakter olaca��
                options.Password.RequiredLength = 6;

                //AlphaNumeric girme zorunlulu�u olmas�n
                options.Password.RequireNonAlphanumeric = true;

                //mutlaka buyuk karakter icersin
                options.Password.RequireUppercase=true;

                //Kullan�c�n�n kac kere yanl�s parola girme hakk� olsun
                options.Lockout.MaxFailedAccessAttempts = 5;

                //failed olduktan sonra login olmas� i�in gereken s�re
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                //Lockout sistemi yeni kullan�c� i�in de ge�erli olsun
                options.Lockout.AllowedForNewUsers = true;

                //tek email ile uyelik olu�turma
                options.User.RequireUniqueEmail=true;

                //mail ve telefon onaylama
                options.SignIn.RequireConfirmedEmail=false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //sessiondaki cookie ayarlar�
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accesdenied";

                //1 G�n boyunca cookiler taray�c�da saklan�r.    
                //options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Varsay�lan de�er 20 dakkad�r

                //uygulama kullan�l�rken cookie kapanma suresi- false olursa uykulama kulan�ls�n ya da kullan�lmas� n20 dakka sonra sona erer
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

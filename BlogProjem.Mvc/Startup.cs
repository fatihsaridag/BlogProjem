using AutoMapper;
using BlogProjem.Data.Concrete.EntityFramework.Contexts;
using BlogProjem.Mvc.Areas.Admin.AutoMapper.Profiles;
using BlogProjem.Services.AutoMapper.Profiles;
using BlogProjem.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProjem.Mvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Sen bir mvc uygulamasý olarak çalýþmalýsýn.
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt => {    //Razor run time sayesinde uygulamada deðiþiklikleri build etmeden  front end de gözlemledik.
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile),typeof(UserProfile));    //Automapperi kayýt ediyoruz.
            services.LoadMyServices();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login");  //Ben kullanýcý giriþi yapmadan admin areaya eriþmek istersek sistem otomatik olarak bu sayfaya yönlendiriyor olacak.
                options.LogoutPath = new PathString("/Admin/User/Logout");  
                options.Cookie = new CookieBuilder
                {
                    //Cookie ayarlarý
                    Name = "BlogProjem",
                    HttpOnly = true, 
                    SameSite = SameSiteMode.Strict ,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();   //Sitemizde bulunmayan view içerisine gitmek istediðimizde bizlere 404 not found uyarýsý vericek.
            }
            //Session : Kullanýcý sisteme giriþ yaptýðýnda serverda oluþturulan oturum. Kullanýcýnýn session durumu devam ettiði sürece bizler kullanýcýyla iligli bilgiler saklýyor olacaðýz.
            app.UseSession();
            app.UseStaticFiles();                               //Statik dosya kullanýmý (resim vs)
            app.UseRouting();                                   //Önce hangi route içerisinde olduðumuzu bilmemiz lazým route - endpoints arasýnda
            app.UseAuthentication();                            //Burada bir kimlik kontrol yaptýk Fatih Sarýdað ile giriþ yaptýk.  
            app.UseAuthorization();                             //Yetki kontrol iþlemini yapýyoruz.(Eðer kullanýcý admin areaya girecekse bi çalýþmasýný istiyoruz.Blogun içindeyse bu iþlemlerden geçmesini istemiyoruz.)
            app.UseEndpoints(endpoints =>
            {
                // ilk uygulama çalýþtýðýnda. Uygulamamýzda tek bir area oldugu için(admin) MapAreaControllerRoute kullandýk. 
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}


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
            //Sen bir mvc uygulamas� olarak �al��mal�s�n.
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt => {    //Razor run time sayesinde uygulamada de�i�iklikleri build etmeden  front end de g�zlemledik.
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile),typeof(UserProfile));    //Automapperi kay�t ediyoruz.
            services.LoadMyServices();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login");  //Ben kullan�c� giri�i yapmadan admin areaya eri�mek istersek sistem otomatik olarak bu sayfaya y�nlendiriyor olacak.
                options.LogoutPath = new PathString("/Admin/User/Logout");  
                options.Cookie = new CookieBuilder
                {
                    //Cookie ayarlar�
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
                app.UseStatusCodePages();   //Sitemizde bulunmayan view i�erisine gitmek istedi�imizde bizlere 404 not found uyar�s� vericek.
            }
            //Session : Kullan�c� sisteme giri� yapt���nda serverda olu�turulan oturum. Kullan�c�n�n session durumu devam etti�i s�rece bizler kullan�c�yla iligli bilgiler sakl�yor olaca��z.
            app.UseSession();
            app.UseStaticFiles();                               //Statik dosya kullan�m� (resim vs)
            app.UseRouting();                                   //�nce hangi route i�erisinde oldu�umuzu bilmemiz laz�m route - endpoints aras�nda
            app.UseAuthentication();                            //Burada bir kimlik kontrol yapt�k Fatih Sar�da� ile giri� yapt�k.  
            app.UseAuthorization();                             //Yetki kontrol i�lemini yap�yoruz.(E�er kullan�c� admin areaya girecekse bi �al��mas�n� istiyoruz.Blogun i�indeyse bu i�lemlerden ge�mesini istemiyoruz.)
            app.UseEndpoints(endpoints =>
            {
                // ilk uygulama �al��t���nda. Uygulamam�zda tek bir area oldugu i�in(admin) MapAreaControllerRoute kulland�k. 
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


using AutoMapper;
using BlogProjem.Data.Concrete.EntityFramework.Contexts;
using BlogProjem.Mvc.Areas.Admin.AutoMapper.Profiles;
using BlogProjem.Mvc.Helpers.Abstract;
using BlogProjem.Mvc.Helpers.Concrete;
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
        public IConfiguration Configuration { get; }    //Appsettings jsonu okumak istiyoruz.

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Sen bir mvc uygulaması olarak çalışmalısın.
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt => {    //Razor run time sayesinde uygulamada değişiklikleri build etmeden  front end de gözlemledik.
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile),typeof(UserProfile));    //Automapperi kayıt ediyoruz.
            services.LoadMyServices(connectionString:Configuration.GetConnectionString("LocalDB"));
            services.AddScoped<IImageHelper, ImageHelper>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login");  //Ben kullanıcı girişi yapmadan admin areaya erişmek istersek sistem otomatik olarak bu sayfaya yönlendiriyor olacak.
                options.LogoutPath = new PathString("/Admin/User/Logout");  
                options.Cookie = new CookieBuilder
                {
                    //Cookie ayarları
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
                app.UseStatusCodePages();   //Sitemizde bulunmayan view içerisine gitmek istediğimizde bizlere 404 not found uyarısı vericek.
            }
            //Session : Kullanıcı sisteme giriş yaptığında serverda oluşturulan oturum. Kullanıcının session durumu devam ettiği sürece bizler kullanıcıyla iligli bilgiler saklıyor olacağız.
            app.UseSession();
            app.UseStaticFiles();                               //Statik dosya kullanımı (resim vs)
            app.UseRouting();                                   //Önce hangi route içerisinde olduğumuzu bilmemiz lazım route - endpoints arasında
            app.UseAuthentication();                            //Burada bir kimlik kontrol yaptık Fatih Sarıdağ ile giriş yaptık.  
            app.UseAuthorization();                             //Yetki kontrol işlemini yapıyoruz.(Eğer kullanıcı admin areaya girecekse bi çalışmasını istiyoruz.Blogun içindeyse bu işlemlerden geçmesini istemiyoruz.)
            app.UseEndpoints(endpoints =>
            {
                // ilk uygulama çalıştığında. Uygulamamızda tek bir area oldugu için(admin) MapAreaControllerRoute kullandık. 
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


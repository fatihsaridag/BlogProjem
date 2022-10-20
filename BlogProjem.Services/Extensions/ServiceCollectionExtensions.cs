using BlogProjem.Data.Abstract;
using BlogProjem.Data.Concrete;
using BlogProjem.Data.Concrete.EntityFramework.Contexts;
using BlogProjem.Entities.Concrete;
using BlogProjem.Services.Abstract;
using BlogProjem.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BlogProjemContext>();
            serviceCollection.AddIdentity<User, Role>(options => 
            {
                //Bizler kullanıcı eklemek istediğimizde bu kullanıcıyla ilgili arka planda ayarlar bulunuyor. Mesela aynı kullanıcıya yönelik email uniq olmalı mı olmamalı mı
                //User Password Options
                options.Password.RequireDigit = false;   //Şifrelerimizde rakam bulunmalı mı bulunmamalı mı?
                options.Password.RequiredLength = 10;    //Şifremzin uzunlugu
                options.Password.RequiredUniqueChars = 0;   //Uniqe karakterlerden kaç tane olması gerekiyor. Mesela =2 deseydik (bir ünlem veya bir soru işareti)
                options.Password.RequireNonAlphanumeric = false;    //Aktif edildiğinde !,@,?,$ gibi özel karakterleri kullanmaya zorunlu kılıp kılmadığı kısım.    
                options.Password.RequireLowercase = false;          //Örnek olarak küçük karakterlerin zorunlu kılınıp kılınmadığı
                options.Password.RequireUppercase = false;          //alper => Alper yazabilir bunu gerçekte true yapmak iyi olur.
                //User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";  //Sadece bu karakterler kullanılabilir.
                options.User.RequireUniqueEmail = true;                     //Bu email adresinden yalnızca sistemde bir tane kullanıcı olmasını sağlar. 

            }).AddEntityFrameworkStores<BlogProjemContext>();      //Burada bir IdentityDbContext belirtmemiz gerekiyor. Identityi burada eklediğimiz için ayarları burada yapıyoruz.
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            return serviceCollection;
        }
    }
}

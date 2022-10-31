using BlogProjem.Data.Concrete.EntityFramework.Mappings;
using BlogProjem.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Data.Concrete.EntityFramework.Contexts
{
    public class BlogProjemContext : IdentityDbContext<User,Role,int,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
        //Bunlar bizim kod tarafında erişip kullanacağımız isimler veri tabanında fluent api ile ayarlıyacağız.
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=BlogProjem;Trusted_Connection=True;Connect Timeout=30;MultipleActiveResultSets=True;");
        //}

        public BlogProjemContext(DbContextOptions<BlogProjemContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new CommentMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleClaimMap());
            modelBuilder.ApplyConfiguration(new UserClaimMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new UserTokenMap());
            modelBuilder.ApplyConfiguration(new UserLoginMap());

        }
    }
}

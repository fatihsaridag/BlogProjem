using BlogProjem.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Data.Concrete.EntityFramework.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).HasMaxLength(150);
            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ViewsCount).IsRequired();
            builder.Property(a => a.CommentCount).IsRequired();
            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);       //Bir kategori birden çok makaleye sahip olur. Ve makale tarafında da foreignjeyi var o da categoryId
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);
            builder.ToTable("Articles");
            //builder.HasData(new Article
            //{
            //    Id = 1,
            //    CategoryId = 1,
            //    Title = "C# 9.0 VE .NET 5 Yenilikleri",
            //    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
            //    Thumbnail = "Default.jpg",
            //    SeoDescription = "C# 9.0 VE .NET 5 Yenilikleri",
            //    SeoTags = "C# , c# 9 , EntityFramework , .Net Core",
            //    SeoAuthor = "Fatih",
            //    Date = DateTime.Now,
            //    IsActive = true,
            //    IsDeleted = false,
            //    CreatedByName = "InitialCreate",
            //    CreatedDate = DateTime.Now,
            //    ModifiedByName = "InitialCreate",
            //    ModifiedDate = DateTime.Now,
            //    Note = "C# 9.0 VE .NET 5 Yenilikleri",
            //    UserId = 1,
            //    ViewsCount = 123,
            //    CommentCount = 1
            //},
            //      new Article
            //      {
            //          Id = 2,
            //          CategoryId = 2,
            //          Title = "C++ 11 VE 19 Yenilikleri",
            //          Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
            //          Thumbnail = "Default.jpg",
            //          SeoDescription = "C++ 11 VE 19 Yenilikleri",
            //          SeoTags = "C++ , c++ 11 , EntityFramework , .Net Core",
            //          SeoAuthor = "Fatih",
            //          Date = DateTime.Now,
            //          IsActive = true,
            //          IsDeleted = false,
            //          CreatedByName = "InitialCreate",
            //          CreatedDate = DateTime.Now,
            //          ModifiedByName = "InitialCreate",
            //          ModifiedDate = DateTime.Now,
            //          Note = "C++ 11 VE 19 Yenilikleri",
            //          UserId = 1,
            //          ViewsCount = 213,
            //          CommentCount = 3
            //      },
            //      new Article
            //      {
            //          Id = 3,
            //          CategoryId = 3,
            //          Title = "JAVASCRIPT ES2022 Yenilikleri",
            //          Content = "Lorem Ipsum is simply dummy text of the printing" +
            //          " and typesetting industry." +
            //          " Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
            //          Thumbnail = "Default.jpg",
            //          SeoDescription = "JAVASCRIPT ES2022 Yenilikleri",
            //          SeoTags = "C++ , c++ 11 , EntityFramework , .Net Core",
            //          SeoAuthor = "Fatih",
            //          Date = DateTime.Now,
            //          IsActive = true,
            //          IsDeleted = false,
            //          CreatedByName = "InitialCreate",
            //          CreatedDate = DateTime.Now,
            //          ModifiedByName = "InitialCreate",
            //          ModifiedDate = DateTime.Now,
            //          Note = "JAVASCRIPT ES2022 Yenilikleri",
            //          UserId = 1,
            //          ViewsCount = 4,
            //          CommentCount = 345
            //      });

        }
    }
}

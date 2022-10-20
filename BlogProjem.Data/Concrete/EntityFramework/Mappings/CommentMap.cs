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
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Text).HasMaxLength(1000);
            builder.Property(c => c.Text).IsRequired();
            builder.HasOne<Article>(a => a.Article).WithMany(c => c.Comments).HasForeignKey(c => c.ArticleId);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.ToTable("Comment");
           // builder.HasData(new Comment
           // {
           //     Id = 1,
           //     ArticleId = 1,
           //     Text = "Lorem Ipsum is simply dummy text of the printing" +
           //    " and typesetting industry." +
           //    " Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
           //     IsActive = true,
           //     IsDeleted = false,
           //     CreatedByName = "InitialCreate",
           //     CreatedDate = DateTime.Now,
           //     ModifiedByName = "InitialCreate",
           //     ModifiedDate = DateTime.Now,
           //     Note = "C# Article Comment"
           // },
           //new Comment
           //{
           //    Id = 2,
           //    ArticleId = 2,
           //    Text = "Lorem Ipsum is simply dummy text of the printing" +
           //    " and typesetting industry." +
           //    " Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
           //    IsActive = true,
           //    IsDeleted = false,
           //    CreatedByName = "InitialCreate",
           //    CreatedDate = DateTime.Now,
           //    ModifiedByName = "InitialCreate",
           //    ModifiedDate = DateTime.Now,
           //    Note = "C++ Article Comment"
           //},
           //new Comment
           //{
           //    Id = 3,
           //    ArticleId = 3,
           //    Text = "Lorem Ipsum is simply dummy text of the printing" +
           //    " and typesetting industry." +
           //    " Lorem Ipsum has been the industry's standard dummy text ever since the 1500s",
           //    IsActive = true,
           //    IsDeleted = false,
           //    CreatedByName = "InitialCreate",
           //    CreatedDate = DateTime.Now,
           //    ModifiedByName = "InitialCreate",
           //    ModifiedDate = DateTime.Now,
           //    Note = "Javascript Article Comment"
           //}
           //);
        }
    }
}

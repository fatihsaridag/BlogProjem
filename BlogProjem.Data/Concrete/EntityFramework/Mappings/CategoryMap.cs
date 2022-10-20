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
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Name).HasMaxLength(50);
            builder.Property(c => c.Description).HasMaxLength(250);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.ToTable("Kategoriler");
           // builder.HasData(
           //new Category
           //{
           //    Id = 1,
           //    Name = "C#",
           //    Description = "C# Programlama Dili ile ilgili en güncel bilgiler.",
           //    IsActive = true,
           //    IsDeleted = false,
           //    CreatedByName = "InitialCreate",
           //    CreatedDate = DateTime.Now,
           //    ModifiedByName = "InitialCreate",
           //    ModifiedDate = DateTime.Now,
           //    Note = "C# Blog kategorisi"

           //},
           //new Category
           //{
           //    Id = 2,
           //    Name = "C++",
           //    Description = "C++Programlama Dili ile ilgili en güncel bilgiler.",
           //    IsActive = true,
           //    IsDeleted = false,
           //    CreatedByName = "InitialCreate",
           //    CreatedDate = DateTime.Now,
           //    ModifiedByName = "InitialCreate",
           //    ModifiedDate = DateTime.Now,
           //    Note = "C++ Blog kategorisi"
           //}
           //,
           //new Category
           //{
           //    Id = 3,
           //    Name = "Javascript",
           //    Description = " Javascript Programlama Dili ile ilgili en güncel bilgiler.",
           //    IsActive = true,
           //    IsDeleted = false,
           //    CreatedByName = "InitialCreate",
           //    CreatedDate = DateTime.Now,
           //    ModifiedByName = "InitialCreate",
           //    ModifiedDate = DateTime.Now,
           //    Note = "Javascript Blog kategorisi"
           //}

           //);
        }
    }
}

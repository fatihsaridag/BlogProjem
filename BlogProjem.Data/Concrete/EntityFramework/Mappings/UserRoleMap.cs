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
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");
            builder.HasData(
            new UserRole
            {
                RoleId = 1,     //İlk id li kullanıcıyı bir admin olarak oluşturmuştuk.
                UserId = 1

            },
             new UserRole
             {
                 RoleId = 2,     //2 id li kullanıcıyı bir admin olarak oluşturmuştuk.
                 UserId = 2

             }
            );
        }
    }
}

using BlogProjem.Data.Abstract;
using BlogProjem.Data.Concrete.EntityFramework.Contexts;
using BlogProjem.Entities.Concrete;
using BlogProjem.Shared.Data.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Data.Concrete.EntityFramework.Repositories
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
    {
        public EfCategoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<Category> GetById(int categoryId)
        {
          return await  BlogProjemContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
        }


        private BlogProjemContext BlogProjemContext { get
            {
                return _context as BlogProjemContext;
            } 
        }

    }
}

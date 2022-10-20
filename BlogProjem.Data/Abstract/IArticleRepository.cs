using BlogProjem.Entities.Concrete;
using BlogProjem.Shared.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Data.Abstract
{
    public interface IArticleRepository : IEntityRepository<Article>
    {
    }
}

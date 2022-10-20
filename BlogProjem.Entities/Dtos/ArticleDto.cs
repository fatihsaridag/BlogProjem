using BlogProjem.Entities.Concrete;
using BlogProjem.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
   public class ArticleDto: DtoGetBase
    {
        public Article Article { get; set; }
    }
}

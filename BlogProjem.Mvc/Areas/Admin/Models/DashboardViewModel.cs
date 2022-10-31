using BlogProjem.Entities.Concrete;
using BlogProjem.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesCount { get; set; }
        public int ArticlesCount { get; set; }      //Silinmemiş makaleler
        public int CommentCount { get; set; }
        public int UserCount { get; set; }
        public ArticleListDto Articles { get; set; }        //Tüm makaleler
    }
}


using BlogProjem.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Concrete
{
    public class Article: EntityBase,IEntity
    {
        public string Title { get; set; }
        public string  Content { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Date { get; set; }
        public int ViewsCount { get; set; }
        public int CommentCount { get; set; }
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public ICollection<Comment> Comments { get; set; }  //Bir makale birden çok yoruma sahip olabilir.
        public int CategoryId { get; set; }                 //Bu makale hangi kategoriye ait
        public Category Category { get; set; }              //Navigation property
        public int UserId { get; set; }                     //Bir makaleyi paylaşan bir tane kullanıcı 
        public User User { get; set; }                      //Navigation property
    }
}

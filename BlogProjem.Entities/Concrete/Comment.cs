using BlogProjem.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Concrete
{
    public class Comment: EntityBase,IEntity
    {
        public string Text { get; set; }    
        public int ArticleId { get; set; }  //Bir yorum bir makaleye ait olmalı 
        public Article Article { get; set; }    //Navigation Property
    }
}

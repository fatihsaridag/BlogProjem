using BlogProjem.Entities.Concrete;
using BlogProjem.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
    public class UserDto : DtoGetBase
    {
        public User User { get; set; }

    }
}

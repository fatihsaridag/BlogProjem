using AutoMapper;
using BlogProjem.Entities.Concrete;
using BlogProjem.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {   
            CreateMap<UserAddDto, User>();          //Bu işlem sonucunda userAddDto sınıfını bir User sınıfına dönüştürmüş oluyoruz.
            CreateMap<User, UserUpdateDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}

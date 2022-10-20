using AutoMapper;
using BlogProjem.Entities.Concrete;
using BlogProjem.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Services.AutoMapper.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<CategoryUpdateDto, Category>().ForMember(dest => dest.ModifiedDate , opt => opt.MapFrom(x => DateTime.Now ));
            CreateMap<Category, CategoryUpdateDto>();   //Bir kategori ,categoryUpdateDto olarak map edilecek.
        }
    }
}

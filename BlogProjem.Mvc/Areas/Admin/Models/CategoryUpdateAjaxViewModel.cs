﻿using BlogProjem.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.Models
{ 
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryUpdateDto CategoryUpdateDto { get; set; }
        public string CategoryUpdatePartial { get; set; } // Biz post işlemi yaptıysak bunun sonucunda bize
                                                       // categoryUpdatePartial geliyor olacak. Örnek olarak kullanıcı açıklama kısmını
                                                       // boş bıraktı oradaki modelin son durumunu bize bildirir. Hemde partial view işlevi
        public CategoryDto CategoryDto { get; set; }  //İşlemler başarılı geldiyse tablonun sol tarafına ekleyeceğiz.
    }
}

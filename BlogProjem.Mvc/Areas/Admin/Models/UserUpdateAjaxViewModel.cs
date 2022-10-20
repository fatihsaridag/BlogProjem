using BlogProjem.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.Models
{ 
    public class UserUpdateAjaxViewModel
    {
        public UserUpdateDto  UserUpdateDto{ get; set; }     
        public string UserUpdatePartial { get; set; } // Biz post işlemi yaptıysak bunun sonucunda bize
                                                       // categoryAddPartial geliyor olacak. Örnek olarak kullanıcı açıklama kısmını
                                                       // boş bıraktı oradaki modelin son durumunu bize bildirir. Hemde partial view işlevi
        public UserDto UserDto { get; set; }  //İşlemler başarılı geldiyse tablonun sol tarafına ekleyeceğiz.
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
    public class UserAddDto
    {
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.
        [MinLength(3, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]
        public string UserName { get; set; }

        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.
        [MinLength(10, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]
        [DataType( DataType.EmailAddress)]


        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.   
        [MinLength(5, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]         //5 Karakter olması Identity Configrasyonunda 5 karakter yaptık.
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(13, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //+905555555555 //13 karakter
        [MinLength(13, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]            
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("Resim")]
        [Required(ErrorMessage = "Lütfen bir {0} seçiniz.")]
        [DataType(DataType.Upload)]                                                    //Upload Olmasına dikkat et!
        public IFormFile  PictureFile { get; set; }                            

        public string  Picture { get; set; }                                    //Bu kısım dosyanın adını saklamak için.
    }
}

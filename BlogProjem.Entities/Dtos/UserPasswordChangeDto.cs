using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
    public class UserPasswordChangeDto
    {
        [DisplayName("Şu Anki Şifreniz")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.   
        [MinLength(5, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]         //5 Karakter olması Identity Configrasyonunda 5 karakter yaptık.
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [DisplayName("Yeni Şifreniz")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.   
        [MinLength(5, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]         //5 Karakter olması Identity Configrasyonunda 5 karakter yaptık.
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Yeni Şifrenizin Tekrarı")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        [MaxLength(30, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.! ")]     //Bizler by değeri fluent api ile 50 olarak girmiştik.   
        [MinLength(5, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır!")]         //5 Karakter olması Identity Configrasyonunda 5 karakter yaptık.
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Girmiş olduğunuz yeni şifreniz ile yeni şifrenizin tekrarı alanları birbiri ile uyuşmalıdır.")]
        public string RepeatPassword { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
    public class CategoryAddDto
    {
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [MaxLength(70, ErrorMessage = "{0} {1} cannot be larger than the character! ")]
        [MinLength(3, ErrorMessage = "{0} {1} cannot be less than a character!")]
        public string Name { get; set; }

        [DisplayName("Category Description")]
        [MaxLength(500, ErrorMessage = "{0} {1} cannot be larger than the character! ")]
        [MinLength(3, ErrorMessage = "{0} {1} cannot be less than a character!")]
        public string Description { get; set; }

        [DisplayName("Category Note")]
        [MaxLength(500, ErrorMessage = "{0} {1} cannot be larger than the character! ")]
        [MinLength(3, ErrorMessage = "{0} {1} cannot be less than a character!")]
        public string Note { get; set; }

        [DisplayName("Is active ? ")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        public bool IsActive { get; set; }
    }
}

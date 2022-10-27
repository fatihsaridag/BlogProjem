using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Entities.Dtos
{
    //Yüklenmiş olan resimlerle ilgili bizlere detaylı bilgi sağlıyor olacak.
    public class ImageUploadedDto
    {
        public string FullName { get; set; }
        public string oldName { get; set; }
        public string Path { get; set; }
        public string FolderName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}

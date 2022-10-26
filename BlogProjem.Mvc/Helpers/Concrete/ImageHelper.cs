using BlogProjem.Entities.Dtos;
using BlogProjem.Mvc.Helpers.Abstract;
using BlogProjem.Shared.Utilities.Extensions;
using BlogProjem.Shared.Utilities.Results.Abstract;
using BlogProjem.Shared.Utilities.Results.ComplexTypes;
using BlogProjem.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Helpers.Concrete

{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;      //Bu işlem bizlere wwwrootun string olarak dosya yolunu vericektir.

        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            //Bir tane dosya yolu oluşturacağız . Ve bu dosya yolu içerisindeki resmi de sileceğiz. 
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}/", pictureName); //Bizlerin bu dosya yolu fileToDelete içerisinde saklanıyor olacak. Bu klasöre eriştik

            if (System.IO.File.Exists(fileToDelete))    //Eğer böyle bir path var ise bu path içerisindeki değerleri silelim. Böyle bir dosya var mı?
            {
                //Böyle bir dosya var ise silme işlemlerini gerçekleştirebiliriz.

                var fileInfo = new FileInfo(fileToDelete);  //Bu dosyanın bilgilerine eriştik
                var imageDeletedDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extensions = fileInfo.Extension,        //Bu bilgileride ImageDeletedDto içerisinde sakladıktan sonra resmin silinmesini sağladık.
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                System.IO.File.Delete(fileToDelete);             //Resmin silinmesini sağladık.
                return new DataResult<ImageDeletedDto>(ResultStatus.Success, imageDeletedDto);          //Sildiğimiz resmin bilgilerini de return ettik.

            }
            else   //Böyle bir klasör bulunmuyorsa.
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error, "Böyle bir resim bulunmadı", null);
            }

        }
        public async Task<IDataResult<UploadedImageDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages")
        {
            //Böyle bir klasör var mı demek
            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))  // Eğer böyle bir klasör yoksa gerekli klasörün if içeirisnde oluşturmamız gerekiyor.
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");  //Yeni bir klasör oluşturduk.
            }
            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);   //Yüklenmiş olan resmin ilk adını alıyoruz.
            string fileExtension = Path.GetExtension(pictureFile.FileName);      //uzntısını almış olduk.
            DateTime dateTime = DateTime.Now;
            // AlperTunga_587_5_38_12_3_10_2020.png
            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}"; //Burada resmin ismini almış olduk. Ve methodla diğer değerleri doldurduk. Extensionu da sona koyarak gerekli dosya adını oluşturduk. Elimizde dosya adı var ama dosya yolunu nereye kaydedicez.
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName); //Dosya yolunu oluşturmuş oluyoruz ve dosyasının adı da bulunuyor.. Kullanıcı resimleri , makale resimleri vs
            await using (var stream = new FileStream(path, FileMode.Create))    //FileStream : Dosyalarla ilgili işlemlerimizi yöneten sınıftır. Nereye kaydedilmeli , hangi modda olacak ? 
            {
                await pictureFile.CopyToAsync(stream); //UserAddDto içerisindeki resmi streame göre kopyaladık.
            }

            return new DataResult<UploadedImageDto>(ResultStatus.Success, $"{userName} adllı kullanıcının resmi başarıyla yüklenmiştir.", new UploadedImageDto { 
                    FullName = $"{folderName}/{newFileName}",
                    oldName = oldFileName,
                    Extension = fileExtension,
                    Path = path,
                    Size = pictureFile.Length       //Long ile return eder.
            }); // AlperTunga_587_5_38_12_3_10_2020.png - "~/img/user.Picture"
        }
    }
}

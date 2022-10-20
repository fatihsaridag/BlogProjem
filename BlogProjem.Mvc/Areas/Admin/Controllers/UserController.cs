using AutoMapper;
using BlogProjem.Entities.Concrete;
using BlogProjem.Entities.Dtos;
using BlogProjem.Mvc.Areas.Admin.Models;
using BlogProjem.Shared.Utilities.Extensions;
using BlogProjem.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> singInManager)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _singInManager = singInManager;
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Index()                       
        {   
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });                                         //Bu işlemi yaptığımızda başarılı bir şekilde UserListDto döndürmüş oluyoruz ve View kısmından da model olarak bunu alıcaz İşleyeceğiz.
        }


        // fatihsaridag26@gmail.com - fatiheses3
        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {

    
                //Öncelikle kontrol etmemiz gereken ilk parametre ModelState durumu . Çünkü kullanıcı yanlış girmiş olabilir.
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email); //Kullanıcının emailiyle kullanıcıyı almış olduk.
                if (user!=null) //Eğer kullanıcı null değilse böyle bir kullanıcı var demektir.
                {
                    var result = await _singInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)   //Bu kullanıcı giriş yapmış mı yapamamış mı bunu öğreniyoruz.
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("","E posta adresiniz veya şifreniz yanlıştır");  //Kullanıcıya bir hata mesajı bırakıyoruz . 
                        return View("UserLogin");

                    }
                }
                else //bizlere bir user gelmeyecektir.
                {
                    ModelState.AddModelError("", "E posta adresiniz veya şifreniz yanlıştır");  //Kullanıcıya bir hata mesajı bırakıyoruz . 
                    return View("UserLogin");

                }
            }
            else
            {
                return View("UserLogin");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto                //Bu bizlere json formatında userlist  dönecek.
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve                           //Bu değeri de verdikten sonra tüm değerleri json formatına dönüştürebiliriz.
            }); 
            return Json(userListDto);                                                   //userListDto yu front ende göndermiş oluyoruz.
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName,userAddDto.PictureFile);                     //Bizlrin UserAddDto daki picture alanına Resim adı eklemesi gerekiyor. Bu resim adını eklersek kullanıcının resmini görüyor olacağız
                var user = _mapper.Map<User>(userAddDto);                                   //UserAddDtoyu usera map ettik. ve artık elimizde bir user mevcut.
                var result = await _userManager.CreateAsync(user, userAddDto.Password);     //UserManager içerisindeki CreateAsync metoduyla kullanıcı ve kullanıcı şifresini paramtere olarak yazıyoruz. Kullanıcı eklenmiş oluyor .  resulta atıyoruz. (IdentityResult)
                if (result.Succeeded)                                                    //Kullanıcı başarıyla eklendi mi eklenmedi 
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel    //Artık yapmamız gereken bizlerin UserAddAjaxModel oluşturması ve bunu da view içerisine dönmesi.
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı adına sahip, kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxModel);
                }   
                else //Identity tarafında hata olsaydı
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    
                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxErrorModel);
                }
            }
            var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorModel);

        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(int userId)
        {
            //Bizlerin kullanıcıyı silebilmek için öncelikle o kullanıcı getirmemiz gerek.
            //Ve o kullanıcıyı getirmek için de userId kullanıyor olacağız.
            var user = await _userManager.FindByIdAsync(userId.ToString());    //Bu method userId yi string olarak istiyor.
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)   //Eğer işlem başarılıysa front end tarafında bir tane userDto gönderelim.
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir.",
                    User = user
                });     //Bu işlemi yaptığımızda artık elimizde json formatında silinmiş bir kullanıcı olacaktır.
                return Json(deletedUser);
            } 
            else      //eğer sucedded olmazsa ? Kullanıcıyı bilgilendirecek bir errormodel dönebiliriz.
            {
                //Her bir hatanın açıklamasını error.Description kısmını bir değişken içerisine atmış oluyoruz. Çünkü bu değişken içerisinde bu hataların bir liste halinde oluşmasını sağlayacak ve daha sonra Dto nun message kısmını hatalarımızı ekliyor olacağız. 
                string errorMessages = String.Empty;
                foreach(var error in result.Errors)
                {
                   errorMessages = $"*{error.Description}\n";         //Her bir hatanın değişkenini errorMessages içerisine atıyoruz.
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcı silinirken bazı hatalar oluştu \n {errorMessages}",
                    User = user

                });
                return Json(deletedUserErrorModel);
            }
        }

        //Neden IActionResult kullanmadıkda partiaViewResult kullandık ? 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);       //userId ye ait kullanıcıyı getirmiş oluyoruz.  Bizlerin artık bir userUpdateDtoua ihtiyacı var.
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);                               //Kızmasının sebebi biz böyle bir profil oluşturmamıştık. 
            return PartialView("_UserUpdatePartial", userUpdateDto);                            //userUpdatePartialı veriyoruz ve model olarak da userUpdateDto yu gönderiyoruz.
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserPicture = oldUser.Picture;   //Resim değişim işlemi gerçekleşmişse . Eski resimi sileceğiz.
                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await  ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    //ImageDeleted(oldUserPicture);
                    isNewPictureUploaded = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)   //Güncelleme için Yeni bir resim eklendi mi ? Evet ise 
                    {
                        ImageDeleted(oldUserPicture);   //Eski resmi sistemimizden sildik. 
                    }
                    var userUpdateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
                    });
                    return Json(userUpdateViewModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)

                    });
                    return Json(userUpdateErrorViewModel);
                }
            }
            else
            {
                var userUpdateModelStateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)

                });
                return Json(userUpdateModelStateErrorViewModel);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ChangeDetails()
        {
            // Kullanıcıyı güncellemek için öncelikle onun bilgilerine erişebilmemiz lazım. Şu anki login olan kullanıcıya erişmemiz yeterli olacaktır.
            var user = await _userManager.GetUserAsync(HttpContext.User);   // Şu an ki kullanıcının bilgilerine erişmiş olduk şimdi bunu bir updateDto ya çevirmemiz gerekiyor.
            var updateDto = _mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;   //Resim değişim işlemi gerçekleşmişse . Eski resimi sileceğiz.
                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    if (oldUserPicture != "default.png")
                    {
                        isNewPictureUploaded = true;
                    }
                    
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)   //Güncelleme için Yeni bir resim eklendi mi ? Evet ise 
                    {
                        ImageDeleted(oldUserPicture);   //Eski resmi sistemimizden sildik. 
                    }
                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto);
                }
            }
            else
            {
                return View(userUpdateDto);
            }
        }

        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid) //Modelimiz buraya doğru bir şekilde geldi ise 
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user,userPasswordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword); //bu işlem identity result dönecek.
                    if (result.Succeeded)   //Eğer şifre başarıyla değiştiyse
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _singInManager.SignOutAsync();
                        await _singInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true,false);
                        TempData.Add("SuccessMessage", "Şifreniz başarıyla değiştirilmiştir.");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Lütfen girmiş olduğunuz şu anki şifrenizi kontrol ediniz.");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
            }

            return View();
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<string> ImageUpload(string userName , IFormFile pictureFile)        //Bu dersimizde upload işlemini tamamlamış oluyo
        {
            // ~/img/user.Picture
            string wwwroot = _env.WebRootPath;      //Bu işlem bizlere wwwrootun string olarak dosya yolunu vericektir.
            // alpertunga     
            // string fileName2 = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            //.png
            string fileExtension = Path.GetExtension(pictureFile.FileName);      //uzntısını almış olduk.
            DateTime dateTime = DateTime.Now;
            // AlperTunga_587_5_38_12_3_10_2020.png
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}"; //Burada resmin ismini almış olduk. Ve methodla diğer değerleri doldurduk. Extensionu da sona koyarak gerekli dosya adını oluşturduk. Elimizde dosya adı var ama dosya yolunu nereye kaydedicez.
            var path = Path.Combine($"{wwwroot}/img", fileName); //Dosya yolunu oluşturmuş oluyoruz ve dosyasının adı da bulunuyor..
            await using (var stream = new FileStream(path, FileMode.Create))    //FileStream : Dosyalarla ilgili işlemlerimizi yöneten sınıftır. Nereye kaydedilmeli , hangi modda olacak ? 
            {
                await pictureFile.CopyToAsync(stream); //UserAddDto içerisindeki resmi streame göre kopyaladık.
            }

            return fileName; // AlperTunga_587_5_38_12_3_10_2020.png - "~/img/user.Picture"
        }


        [Authorize(Roles = "Admin,Editor")]
        public bool ImageDeleted(string pictureName)
        {
            //Bir tane dosya yolu oluşturacağız . Ve bu dosya yolu içerisindeki resmi de sileceğiz. 
            string wwwroot = _env.WebRootPath;  //Elimizde hem dosya yolunun adı hemde www root klasörünün dosya yolu olduguna göre bunların ikisini kombin ederek buradaki resmin dosya yolunu oluşturalım.
            var fileToDelete = Path.Combine($"{wwwroot}/img", pictureName); //Bizlerin bu dosya yolu fileToDelete içerisinde saklanıyor olacak.
            if (System.IO.File.Exists(fileToDelete))    //Eğer böyle bir path var ise bu path içerisindeki değerleri silelim.
            {
                System.IO.File.Delete(fileToDelete);    //Lütfen fileToDelete in içerisindeki dosya yoluna gidip buradaki dosyayı siler misin
                return true;                            //İşlem evet
            }
            else
            {
                return false;

            }
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }

    }   
}

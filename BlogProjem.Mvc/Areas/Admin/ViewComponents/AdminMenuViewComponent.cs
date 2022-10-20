using BlogProjem.Entities.Concrete;
using BlogProjem.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public ViewViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;  //Hangi kullanıcı login olmuşsa onu getirmek için http userı kullanıyoruz.
            var roles = _userManager.GetRolesAsync(user).Result;    //Rolleri alıyoruz.
            return View(new UserWithRolesViewModel
            {
                User = user,
                Roles = roles
            }); 
            //Admin menüsü yüklendiği zaman içerisindeki model iile gelecektir.

        }
    }
}

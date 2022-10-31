using BlogProjem.Data.Concrete.EntityFramework.Contexts;
using BlogProjem.Entities.Concrete;
using BlogProjem.Mvc.Areas.Admin.Models;
using BlogProjem.Services.Abstract;
using BlogProjem.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjem.Mvc.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager , ICategoryService categoryService , ICommentService commentService , IArticleService articleService)
        {
            _userManager = userManager;
            _articleService = articleService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByNonDeletedAsync();
            var articlesCountResult = await _articleService.CountByNonDeletedAsync();
            var commentsCountResult = await _commentService.CountByNonDeletedAsync();
            var usersCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();
            if (categoriesCountResult.ResultStatus == ResultStatus.Success && articlesCountResult.ResultStatus == ResultStatus.Success && commentsCountResult.ResultStatus == ResultStatus.Success && usersCount > -1 && articlesCountResult.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    CategoriesCount = categoriesCountResult.Data,
                    ArticlesCount = articlesCountResult.Data,
                    CommentCount = commentsCountResult.Data,
                    UserCount = usersCount,
                    Articles = articlesResult.Data
                });

            }
            return NotFound();

        }
    }
}

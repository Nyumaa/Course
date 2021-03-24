using Course_Project.Data.Repository;
using Course_Project.Data.UserService;
using Course_Project.Models;
using Course_Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        private readonly IUserService _user;


        public HomeController(ILogger<HomeController> logger, IRepository repository, IUserService user)
        {
            _logger = logger;
            _repo = repository;
            _user = user;
        }
        public IActionResult Index(int pageNumber)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1 });

            IndexViewModel vm = _repo.GetAllPosts(pageNumber);
            vm.TopFivePosts = _repo.GetTopFivePosts();
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[controller]/[action]/{username}")]
        public IActionResult CreateByAdmin(string username)
        {
            string name = username;
            if (string.IsNullOrEmpty(name))
                name = User.Identity.Name;
            return View("Create", new CreateViewModel { Author = name });
            
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateViewModel { Author = User.Identity.Name });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = vm.Id,
                    Title = vm.Title,
                    Category = vm.Category,
                    Description = vm.Description,
                    Size = vm.Size,
                    AgeRaiting = vm.AgeRaiting,
                    Author = _user.GetIdByName(vm.Author),
                    Tags = vm.Tags
                };

                _repo.AddPost(post);
                if (await _repo.SaveChangesAsync())
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Проверьте введеную информацию и попробуйте еще раз!");
                    return View("Create", vm);
                }  
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Заполните все поля!");
                return View("Create", vm);
            }
        }
        [HttpGet("[action]/{id}")]
        public IActionResult Post(int id)
        {
            Post post = _repo.GetPost(id);
            if (post != null)
            {
                User author = _user.GetById(post.Author);
                return View(new PostViewModel 
                {
                    Id = post.Id,
                    Title = post.Title,
                    Category = post.Category,
                    Description = post.Description,
                    AgeRaiting = post.AgeRaiting,
                    Size = post.Size,
                    Author = author.UserName,
                    Chapters = post.Chapters,
                    ImageUrl = author.ImageUrl,
                    Tags = post.Tags,
                    Comments = post.Comments
                });
            }
            else
                return RedirectToAction("Index"); 
        }

        [HttpGet("Post/{id}/[action]")]
        public IActionResult AddChapter(int id)
        {
            return View("CreateChapter", new CreateChapterViewModel { PostId = id });
        }
        public async Task<IActionResult> CreateChapter(CreateChapterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var chapter = new Chapter
                {
                    Note = vm.Note,
                    Title = vm.Title,
                    Body = vm.Body
                };
                var post = _repo.GetPost(vm.PostId);
                post.Chapters.Add(chapter);

                _repo.UpdatePost(post);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Chapter", new { id = vm.PostId , idC = vm.PostId});
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Заполните все поля!");
                return View("Index");
            }
        }
        [HttpGet("Post/{id}/[action]/{idC}")]
        public IActionResult Chapter(int id, int idC)
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });

            var post = _repo.GetPost(vm.PostId);
            Comment comment = new Comment
            {
                Creator = vm.Author,
                Message = vm.Message
            };
            post.Comments.Add(comment);
            _repo.UpdatePost(post);
            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = vm.PostId });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

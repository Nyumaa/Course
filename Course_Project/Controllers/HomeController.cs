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
        public IActionResult Index(int pageNumber, string search)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1 });

            IndexViewModel vm = _repo.GetAllPosts(pageNumber, search);
            vm.TopFivePosts = _repo.GetTopFivePosts();
            vm.Tags = _repo.GetAllTags();
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[controller]/[action]/{username}")]
        public IActionResult CreateByAdmin(string username)
        {
            string name = username;
            if (string.IsNullOrEmpty(name))
                name = User.Identity.Name;
            return View("Create", new CreateViewModel { Author = name, WhiteList = _repo.GetAllTags() });
            
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateViewModel { Author = User.Identity.Name , WhiteList = _repo.GetAllTags()});
        }

        [HttpGet("Post/{action}/{id}")]
        public IActionResult Edit(int id)
        {
            Post post = _repo.GetPost(id);
            if (User.Identity.Name == _user.GetNameById(post.Author) || User.IsInRole("Admin"))
            {
                return View(new EditViewModel { Id = post.Id, Description = post.Description, AgeRaiting = post.AgeRaiting, Size = post.Size, Tags = post.Tags, Title = post.Title, WhiteList = _repo.GetAllTags() });
            }
            else
                return RedirectToAction("Index");
            
        }

        [HttpPost]
        public async Task<IActionResult>EditPost(EditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var post = _repo.GetPost(vm.Id);
                post.Title = vm.Title;
                post.Description = vm.Description;
                post.AgeRaiting = vm.AgeRaiting;
                post.Size = vm.Size;
                post.Tags = vm.Tags;

                _repo.UpdatePost(post);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Post", new { id = vm.Id });
            }
            else
            {
                vm.WhiteList = _repo.GetAllTags();
                return View("Edit", vm);
            }
                
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
                    Raiting = 0,
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
                    vm.WhiteList = _repo.GetAllTags();
                    return View("Create", vm);
                }  
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Заполните все поля!");
                vm.WhiteList = _repo.GetAllTags();
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
                    Comments = post.Comments,
                    Raiting = post.Raiting,
                    Raitings = post.Raitings
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
                return View("CreateChapter", vm);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteChapter(int idPost, int idChapter)
        {
            Post post = _repo.GetPost(idPost);
            if(_user.GetNameById(post.Author) == User.Identity.Name || User.IsInRole("Admin"))
            {
                Chapter chapter = post.Chapters.FirstOrDefault(c => c.Id == idChapter);
                if(chapter != null)
                {
                    post.Chapters.Remove(chapter);
                    _repo.UpdatePost(post);
                    await _repo.SaveChangesAsync();
                }
            }

            return Json(new { redirectToUrl = Url.Action("Post", new { id = idPost }) });
        }
        [HttpGet("Post/{id}/[action]/{idC}")]
        public IActionResult Chapter(int id, int idC)
        {
            Chapter chapter = _repo.GetChapterOnPost(id, idC);
            if (chapter == null)
                return RedirectToAction("Index");

            ChapterViewModel vm = new ChapterViewModel
            {
                Id = idC,
                Body = chapter.Body,
                Note = chapter.Note,
                PostId = id,
                Title = chapter.Title,
                Likes = chapter.Likes,
                LikeReady = chapter.Likes.FirstOrDefault(x => x.Author == _user.GetIdByName(User.Identity.Name)) != null ? true : false 
                
            };
            return View(vm);
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

        [Authorize]
        [HttpPost]
        public async Task AddRaiting(int id, int value)
        {
            var post = _repo.GetPost(id);
            post.Raitings.Add(new Raiting { Author = _user.GetIdByName(User.Identity.Name), Value = value });
            _repo.RecalculateRaiting(ref post);
            _repo.UpdatePost(post);
            await _repo.SaveChangesAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task AddLikeOnChapter(int id, int idC) 
        {
            var post = _repo.GetPost(id);
            post.Chapters.Where(c => c.Id == idC).ToList()[0].Likes.Add(new Like { Author = _user.GetIdByName(User.Identity.Name) });
            _repo.UpdatePost(post);
            await _repo.SaveChangesAsync();
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

        [HttpPost]
        public IActionResult ChangeTheme(string url)
        {
            string cookie = Request.Cookies["theme"];
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            if (cookie == null || cookie == "light")
            {
                Response.Cookies.Append("theme", "dark", options);
            }
            else
            {
                Response.Cookies.Append("theme", "light", options);
            }
            return Json(new { redirectToUrl = LocalRedirect(url).Url});


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

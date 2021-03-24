using Course_Project.Data.Repository;
using Course_Project.Data.UserService;
using Course_Project.Models;
using Course_Project.Models.Profile;
using Course_Project.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRepository _repo;
        public ProfileController(IUserService userService, IRepository repo)
        {
            _userService = userService;
            _repo = repo;
        }
        [HttpGet("[controller]/{username}")]
        public async Task<IActionResult> Detail(string username)
        {
            var user = _userService.GetByUserName(username);
            ProfileViewModel model = new ProfileViewModel();
            if (user != null)
            {
                List<Post> posts = await _repo.GetLastFivePosts(user.Id);
                List<PostViewModel> postsVM = new List<PostViewModel>();
                posts.ForEach(i => postsVM.Add(new PostViewModel {Id = i.Id, Title = i.Title, AgeRaiting = i.AgeRaiting, Size = i.Size, Category = i.Category, Author = username , Tags = i.Tags}));
                model = new ProfileViewModel()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl,
                    AboutMe = user.AboutMe,
                    Posts = postsVM
                };
            }
            else
                return RedirectToAction("Index", "Home");
            
            return View(model);
        }
    }
}

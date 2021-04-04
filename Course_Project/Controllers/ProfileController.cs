using Course_Project.Data.CloudStorage;
using Course_Project.Data.Repository;
using Course_Project.Data.UserService;
using Course_Project.Models;
using Course_Project.Models.Profile;
using Course_Project.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRepository _repo;
        private readonly ICloudStorage _cloud;
        public ProfileController(IUserService userService, IRepository repo, ICloudStorage cloud)
        {
            _userService = userService;
            _repo = repo;
            _cloud = cloud;
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

        [HttpPost]
        public async Task UploadImage()
        {
            var file = Request.Form.Files[0];
            if (file.FileName.Split('.').Last() == "png" || file.FileName.Split('.').Last() == "jpg")
            {
                User user = _userService.GetByUserName(User.Identity.Name);
                user.ImageUrl = await _cloud.UploadFileAsync(file, DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss_") + file.FileName);
                await _userService.Update(user);
            }
        }

        [HttpPost]
        public async Task UpdateAboutMe(string username, string text)
        {
            User user = _userService.GetByUserName(username);
            if(user.UserName == User.Identity.Name || User.IsInRole(User.Identity.Name))
            {
                user.AboutMe = text;
                await _userService.Update(user);
            }
        }
    }
}

using Course_Project.Data.Repository;
using Course_Project.Data.UserService;
using Course_Project.Models;
using Course_Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private readonly IUserService _userService;
        public PanelController(IRepository repo, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }
        [HttpGet("[controller]/{pageNumber}")]
        public IActionResult Index(int pageNumber)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1 });
            return View(_userService.GetAll(pageNumber));
        }
        [HttpGet("[controller]/[action]/{users}")]
        public async Task Block(string users)
        {
            if (!string.IsNullOrEmpty(users))
            {
                await _userService.ChangeStatusUser(users.Split("|"), "Blocked");
            }

        }
        [HttpGet("[controller]/[action]/{users}")]
        public async Task Unblock(string users)
        {
            if (!string.IsNullOrEmpty(users))
            {
                await _userService.ChangeStatusUser(users.Split("|"), "Active");
            }
        }
        [HttpGet("[controller]/[action]/{users}")]
        public async Task Delete(string users)
        {
            if (!string.IsNullOrEmpty(users))
            {
                await _userService.DeleteUsers(users.Split("|"));
            }
        }

        [HttpGet("[controller]/[action]/{users}")]
        public async Task RaiseToAdmin(string users)
        {
            if (!string.IsNullOrEmpty(users))
            {
                await _userService.AddNewRole(users.Split("|"), "Admin");
            }
        }

        [HttpGet("[controller]/[action]/{users}")]
        public async Task RemoveAdminRights(string users)
        {
            if (!string.IsNullOrEmpty(users))
            {
                await _userService.DeleteRole(users.Split("|"), "Admin");
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new CreateViewModel());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new CreateViewModel
                {
                    Id = post.Id,
                    Description = post.Description,
                    Category = post.Category,
                    Tags = post.Tags,
                    Title = post.Title
                });
            }
        }
    }

    
}

using Course_Project.Models;
using Course_Project.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Data.UserService
{
    public class UserService: IUserService
    {
        private AppDbContext _ctx;
        private readonly UserManager<User> _userManager;
        public UserService(AppDbContext ctx, UserManager<User> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public User GetById(string id)
        {
            return _ctx.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetByUserName(string username)
        {
            return _ctx.Users.FirstOrDefault(x => x.UserName == username);
        }
        public List<User> GetAll()
        {
            return _ctx.Users.ToList();
        }
        public string GetNameById(string id)
        {
            return GetById(id).UserName;
        }
        public string GetIdByName(string name)
        {
            return GetByUserName(name).Id;
        }
        public async Task ChangeStatusUser(string[] users, string status)
        {
            foreach(var user in users)
            {
                User profile = _userManager.FindByIdAsync(user).Result;
                profile.Status = status;
                await _userManager.UpdateAsync(profile);
                
                if(status == "Blocked")
                    await _userManager.UpdateSecurityStampAsync(profile);
            }
        }

        public async Task AddNewRole(string[] users, string role)
        {
            foreach (var user in users)
            {
                User profile = _userManager.FindByIdAsync(user).Result;
                await _userManager.AddToRoleAsync(profile, role);
                await _userManager.UpdateAsync(profile);
            }
        }

        public async Task DeleteRole(string[] users, string role)
        {
            foreach(var user in users)
            {
                User profile = _userManager.FindByIdAsync(user).Result;
                await _userManager.RemoveFromRoleAsync(profile, role);
                await _userManager.UpdateAsync(profile);
                await _userManager.UpdateSecurityStampAsync(profile);
            }
        }
        
        public async Task DeleteUsers(string[] users)
        {
            foreach(var user in users)
            {
                await _userManager.DeleteAsync(_userManager.FindByIdAsync(user).Result);
            }
        }
        public PanelViewModel GetAll(int pageNumber)
        {
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Users.ToList();

            int usersCount = query.Count();
            int pageCount = (int)Math.Ceiling(usersCount * 1.0 / pageSize);

            List<User> users = query
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            return new PanelViewModel
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = usersCount > skipAmount + pageSize,
                Pages = pageNumbers(pageNumber, pageCount),
                Users = users
            };
        }

        public IEnumerable<int> pageNumbers(int pageNumber, int pageCount)
        {
            if (pageCount <= 5)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    yield return i;
                }
            }
            else
            {
                int midPoint = pageNumber < 3 ? 3
                : pageNumber > pageCount - 2 ? pageCount - 2
                : pageNumber;

                int lowerBound = midPoint - 2;
                int upperBound = midPoint + 2;

                if(lowerBound != 1){
                    yield return 1;
                    if(lowerBound - 1 > 1)
                    {
                        yield return -1;
                    }
                }

                for(var i = lowerBound; i <= upperBound; i++)
                {
                    yield return i;
                }
                if(upperBound != pageCount)
                {
                    if(pageCount - upperBound > 1)
                    {
                        yield return -1;
                    }
                    yield return pageCount;
                }
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}

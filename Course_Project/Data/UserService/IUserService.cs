using Course_Project.Models;
using Course_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Data.UserService
{
    public interface IUserService
    {
        User GetById(string id);
        User GetByUserName(string userName);
        List<User> GetAll();
        PanelViewModel GetAll(int pageNumber);
        string GetNameById(string id);
        string GetIdByName(string name);
        Task ChangeStatusUser(string[] users, string state);
        Task AddNewRole(string[] users, string role);
        Task DeleteRole(string[] users, string role);
        Task DeleteUsers(string[] users);
        Task<bool> SaveChangesAsync();
    }
}

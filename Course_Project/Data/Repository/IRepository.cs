using Course_Project.Models;
using Course_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        List<Tag> GetAllTags();
        Task<List<Post>> GetLastFivePosts(string author);
        List<Post> GetTopFivePosts();
        IndexViewModel GetAllPosts(int pageNumber, string search);
        Chapter GetChapterOnPost(int idPost, int idChapter);
        void AddPost(Post post);
        void RecalculateRaiting(ref Post post);
        void UpdatePost(Post post);
        Task<bool> SaveChangesAsync();
    }
}

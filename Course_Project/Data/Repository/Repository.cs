using Course_Project.Models;
using Course_Project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;
        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public Post GetPost(int id)
        {
            return _ctx.Posts
                 .Include(p => p.Comments)
                 .Include(p => p.Chapters)
                 .FirstOrDefault(p => p.Id == id);
        }

        public async Task<List<Post>> GetLastFivePosts(string author)
        {
            return await _ctx.Posts.Where(x => x.Author == author).OrderByDescending(x=> x.Created).Take(5).ToListAsync();
        }
        public List<Post> GetTopFivePosts()
        {
            return _ctx.Posts.OrderBy(x => x.Raiting).Take(5).ToList();
        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }
        public void AddPost(Post post)
        {
            AddTags(post.Tags);
            _ctx.Posts.Add(post);
        }
        public IndexViewModel GetAllPosts(int pageNumber)
        {
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = GetAllPosts(); 

            int usersCount = query.Count();
            int pageCount = (int)Math.Ceiling(usersCount * 1.0 / pageSize);

            List<Post> posts = query
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            return new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = usersCount > skipAmount + pageSize,
                Pages = pageNumbers(pageNumber, pageCount),
                Posts = posts
            };
        }
        private void AddTags(string tags)
        {
            List<string> words = tags.Split(',').ToList();
            foreach(var word in words)
            {
                Tag tag = _ctx.Tags.FirstOrDefault(x => x.Name == word);
                if (tag != null)
                    tag.Used++;
                else
                    _ctx.Tags.Add(new Tag { Name = word , Used = 1});
            }               
        }
        private IEnumerable<int> pageNumbers(int pageNumber, int pageCount)
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


                if (lowerBound != 1)
                {
                    yield return 1;
                    if (lowerBound - 1 > 1)
                    {
                        yield return -1;
                    }
                }

                for (int i = lowerBound; i <= upperBound; i++)
                {
                    yield return i;
                }
                if (upperBound != pageCount)
                {
                    if (pageCount - upperBound > 1)
                    {
                        yield return -1;
                    }
                    yield return pageCount;
                }
            }
        }
        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}

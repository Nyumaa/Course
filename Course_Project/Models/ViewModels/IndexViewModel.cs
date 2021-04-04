using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.ViewModels
{
    public class IndexViewModel
    {
        public int PageNumber { get; set; }
        public bool NextPage { get; set; }
        public int PageCount { get; set; }
        public string Category { get; set; }
        public string Search { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Post> TopFivePosts { get; set; }
        public IEnumerable<int> Pages { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}

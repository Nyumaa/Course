using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public List<Like> Likes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.ViewModels
{
    public class ChapterViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public string Body { get; set; }
        public List<Like> Likes { get; set; }
        public bool LikeReady { get; set; }

    }
}

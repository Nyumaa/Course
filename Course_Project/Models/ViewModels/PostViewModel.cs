using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AgeRaiting { get; set; }
        public float Raiting { get; set; }
        public string Tags { get; set; }
        public string Size { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<Raiting> Raitings { get; set; }
    }
}

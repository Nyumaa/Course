using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [MaxLength(255)]
        public string Tags { get; set; }
        [MaxLength(255)]
        public string Category { get; set; }
        public string Size { get; set; }
        public string AgeRaiting { get; set; }
        public float Raiting { get; set; }
        [MaxLength(255)]
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public List<Chapter> Chapters { get; set; }
        public List<Comment> Comments { get; set; }
    }
}

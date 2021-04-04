using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.ViewModels
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Size { get; set; }
        public string AgeRaiting { get; set; }
        [Required]
        public string Tags { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public List<Tag> WhiteList { get; set; }
    }
}

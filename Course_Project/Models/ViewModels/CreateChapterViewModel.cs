using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.ViewModels
{
    public class CreateChapterViewModel
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Note { get; set; }
        [Required]
        public string Body { get; set; }
    }
}

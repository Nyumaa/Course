using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models
{
    public class User : IdentityUser
    {
        public virtual DateTime? RegistrationDate { get; set; } = DateTime.Now;
        public virtual string ImageUrl { get; set; } = "https://storage.googleapis.com/fancourse/ninja.png";
        [MaxLength(255)]
        public virtual string FirstName { get; set; }
        public virtual string AboutMe { get; set; } = "Автор ничего не рассказал о себе!";
        [MaxLength(255)]
        public virtual string LastName { get; set; }
        public virtual DateTime DateBirth { get; set; }
        [MaxLength(255)]
        public virtual string Status { get; set; } = "Active";
    }
}

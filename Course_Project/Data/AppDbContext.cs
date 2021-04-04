using Course_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option): base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().HasIndex(p => p.Id).IsUnique().IncludeProperties(p => p.Description);
            base.OnModelCreating(builder);
        }


        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}

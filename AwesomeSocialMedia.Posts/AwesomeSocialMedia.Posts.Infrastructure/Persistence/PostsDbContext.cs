using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeSocialMedia.Posts.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeSocialMedia.Posts.Infrastructure.Persistence
{
    public class PostsDbContext : DbContext
    {
        public PostsDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Post> Post { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            builder.Entity<Post>(e => 
            {
                e.HasKey(p => p.Id);

                e.Ignore(p => p.Events);
            });
        }
    }
}
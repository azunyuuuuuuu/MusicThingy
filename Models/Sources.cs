using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MusicThingy.Models
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appdata.db");
        }

        public DbSet<YouTubeSource> Sources { get; set; }
        public DbSet<YouTubeVideo> Videos { get; set; }
    }

    public class YouTubeSource
    {
        public Guid YouTubeSourceId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public List<YouTubeVideo> Videos { get; set; } = new List<YouTubeVideo>();
    }

    public class YouTubeVideo
    {
        public Guid YouTubeVideoId { get; set; }
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}

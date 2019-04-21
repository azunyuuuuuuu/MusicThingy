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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceVideo>()
                .HasKey(x => new { x.SourceId, x.VideoId });

            modelBuilder.Entity<SourceVideo>()
                .HasOne(x => x.Source)
                .WithMany(x => x.SourceVideos)
                .HasForeignKey(x => x.SourceId);

            modelBuilder.Entity<SourceVideo>()
                .HasOne(x => x.Video)
                .WithMany(x => x.SourceVideos)
                .HasForeignKey(x => x.VideoId);
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Video> Videos { get; set; }
    }

    public class Source
    {
        public Guid SourceId { get; set; }
        public string Title { get; set; }
        public string PlaylistId { get; set; }
        public string Author { get; set; }
        public List<SourceVideo> SourceVideos { get; set; } = new List<SourceVideo>();
        public string Description { get; set; }
    }

    public class Video
    {
        public Guid VideoId { get; set; }
        public string YouTubeId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        // public List<string> Keywords { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTimeOffset UploadDate { get; set; }
        public List<SourceVideo> SourceVideos { get; set; } = new List<SourceVideo>();
    }

    public class SourceVideo
    {
        public Guid SourceId { get; set; }
        public Source Source { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
    }
}

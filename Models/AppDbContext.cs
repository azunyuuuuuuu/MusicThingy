﻿using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MusicThingy.Models
{
    public class AppDbContext : DbContext
    {
        private readonly string databasepath;

        public AppDbContext(IConfiguration config)
        {
            databasepath = Path.Combine(config.GetSection("config").Get<Configuration>().DataPath, "database.sqlite");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={databasepath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YouTubeSourceMedia>();

            modelBuilder.Entity<SourceMedia>()
                .HasKey(x => new { x.SourceId, x.MediaId });
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Media> Media { get; set; }
    }
}

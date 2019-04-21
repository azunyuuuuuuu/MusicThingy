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
}

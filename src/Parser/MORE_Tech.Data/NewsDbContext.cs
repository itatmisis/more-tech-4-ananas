using Microsoft.EntityFrameworkCore;
using MORE_Tech.Data.Configurations;
using MORE_Tech.Data.Models;

namespace MORE_Tech.Data
{
    public class NewsDbContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<NewsSource> NewsSources { get; set; }
        public DbSet<Attachments> Attachments { get; set; }

        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           

            builder.ApplyConfiguration(new AttachmentsConfiguration());
            builder.ApplyConfiguration(new NewsConfiguration());
            builder.ApplyConfiguration(new NewsSourceConfiguration());
        }
    }
}

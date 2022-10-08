using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Configurations
{
    internal class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder
                .HasKey(x => x.Id);



            builder
                .HasOne<NewsSource>(x => x.Source)
                .WithMany(x => x.News)
                .HasForeignKey(x => x.SourceId);
        }
    }
}

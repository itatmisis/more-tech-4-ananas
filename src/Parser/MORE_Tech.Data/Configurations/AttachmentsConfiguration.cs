
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Configurations
{
    internal class AttachmentsConfiguration : IEntityTypeConfiguration<Attachments>
    {
        public void Configure(EntityTypeBuilder<Attachments> builder)
        {
            builder
                .HasOne<News>(x => x.News)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.NewsId);
        }
    }
}

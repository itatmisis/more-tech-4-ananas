using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Configurations
{
    internal class NewsSourceConfiguration : IEntityTypeConfiguration<NewsSource>
    {
        public void Configure(EntityTypeBuilder<NewsSource> builder)
        {
            builder.ToTable("sources");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.IsActive)
                .HasColumnName("is_active");
        }
    }
}

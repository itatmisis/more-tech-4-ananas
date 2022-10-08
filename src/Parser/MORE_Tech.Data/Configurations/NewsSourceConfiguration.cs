using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Configurations
{
    internal class NewsSourceConfiguration : IEntityTypeConfiguration<NewsSource>
    {
        public void Configure(EntityTypeBuilder<NewsSource> builder)
        {
            builder.ToTable("newssources");

            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();




        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Data.EF.Extensions;
using NetCore.Data.Entities;

namespace NetCore.Data.EF.Configurations
{
    public class BlogTagConfiguration : DbEntityConfiguration<BlogTag>
    {
        public override void Configure(EntityTypeBuilder<BlogTag> entity)
        {
            entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
            .HasColumnType("varchar(50)");
            // etc.
        }
    }
}
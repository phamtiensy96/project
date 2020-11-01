using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.EF.Extensions;
using NetCore.Data.Entities;

namespace NetCore.Data.EF.Configurations
{
    public class AdvertistmentPositionConfiguration : DbEntityConfiguration<AdvertistmentPosition>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPosition> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired();
            // etc.
        }
    }
}

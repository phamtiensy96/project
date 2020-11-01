using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.EF.Extensions;
using NetCore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace NetCore.Data.EF.Configurations
{
    public class AdvertistmentPageConfiguration : DbEntityConfiguration<AdvertistmentPage>
    {
        public override void Configure(EntityTypeBuilder<AdvertistmentPage> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(20).IsRequired().HasColumnType("nvarchar(20)"); ;
            // etc.
        }
    }
}

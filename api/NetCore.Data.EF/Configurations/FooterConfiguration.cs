using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.EF.Extensions;
using NetCore.Data.Entities;

namespace NetCore.Data.EF.Configurations
{
   public class FooterConfiguration : DbEntityConfiguration<Footer>
    {
        public override void Configure(EntityTypeBuilder<Footer> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasMaxLength(255)
                .HasColumnType("varchar(255)").IsRequired();
            // etc.
        }
    }
}

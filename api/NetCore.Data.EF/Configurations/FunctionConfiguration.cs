﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.EF.Extensions;
using NetCore.Data.Entities;

namespace NetCore.Data.EF.Configurations
{
    public class FunctionConfiguration : DbEntityConfiguration<Function>
    {
        public override void Configure(EntityTypeBuilder<Function> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired()
            .HasColumnType("varchar(128)");
            // etc.
        }
    }
}

// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Weaving.Domain.ReadModels;

namespace Weaving.Data.EntityFrameworkCore
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManufactureOrderReadModel>(etb =>
              {
                  etb.ToTable("Weaving_Orders");
                  etb.HasKey(e => e.Identity);

                  etb.Property(p => p.BlendedJson).HasMaxLength(255);
                  etb.Property(p => p.YarnCodesJson).HasMaxLength(255);

                  etb.ApplyAuditTrail();
                  etb.ApplySoftDelete();
              }
            );
        }
    }
}
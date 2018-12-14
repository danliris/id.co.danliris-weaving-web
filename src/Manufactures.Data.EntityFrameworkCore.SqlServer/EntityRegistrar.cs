// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Manufactures.Domain.ReadModels;
using Manufactures.Domain.Entities;

namespace Manufactures.Data.EntityFrameworkCore
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

                  etb.HasOne(p => p.Composition).WithMany().HasForeignKey(k => k.CompositionId);

                  etb.ApplyAuditTrail();
                  etb.ApplySoftDelete();
              }
            );


            modelBuilder.Entity<GoodsComposition>(etb =>
            {
                etb.ToTable("GoodsConstruction");

                etb.HasKey(e => e.Identity);

                etb.Ignore(p => p.MaterialIds);
                etb.Property(p => p.MaterialIdsJson).HasMaxLength(500);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
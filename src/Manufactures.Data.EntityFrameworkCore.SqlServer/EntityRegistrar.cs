// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Orders.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Data.EntityFrameworkCore
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConstructionDetail>(etb =>
            {
                etb.ToTable("Weaving_ConstructionDetails");
                etb.HasKey(e => e.Identity);
                
                etb.Property(e => e.Yarn).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<ConstructionDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_Constructions");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.ConstructionNumber).HasMaxLength(255);
                etb.Property(p => p.WovenType).HasMaxLength(255);
                etb.Property(p => p.WarpType).HasMaxLength(255);
                etb.Property(p => p.WeftType).HasMaxLength(255);

                etb.HasMany(e => e.ConstructionDetails)
                                  .WithOne(p => p.ConstructionDocument)
                                  .HasForeignKey(f => f.ConstructionDocumentId);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MaterialTypeReadModel>(etb =>
            {
                etb.ToTable("Weaving_MaterialTypes");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Code).HasMaxLength(255);
                etb.Property(p => p.Name).HasMaxLength(255);
                etb.Property(p => p.Description).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<WeavingOrderDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_OrderDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.FabricConstructionDocument).HasMaxLength(255);
                etb.Property(p => p.Period).HasMaxLength(255);
                etb.Property(p => p.WeavingUnit).HasMaxLength(255);
                etb.Property(p => p.Composition).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
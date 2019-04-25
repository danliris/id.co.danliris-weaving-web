// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Manufactures.Domain.Construction.ReadModels;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.YarnNumbers.ReadModels;
using Manufactures.Domain.Suppliers.ReadModels;
using Manufactures.Domain.Yarns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.MachineTypes.ReadModels;
using Manufactures.Domain.OperationalMachinesPlanning.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shifts.ReadModels;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;

namespace Manufactures.Data.EntityFrameworkCore
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyOperationSizingDetail>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingDetails");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.Visco).HasMaxLength(255);
                etb.Property(e => e.Information).HasMaxLength(2000);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationSizingReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingDocuments");
                etb.HasKey(e => e.Identity);

                etb.HasMany(e => e.DailyOperationSizingDetails)
                    .WithOne(e => e.DailyOperationSizingDocument)
                    .HasForeignKey(e => e.DailyOperationSizingDocumentId);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<OperatorReadModel>(etb =>
            {
                etb.ToTable("Weaving_OperatorDocuments");
                etb.HasKey(e => e.Identity);
                etb.Property(e => e.Assignment).HasMaxLength(255);
                etb.Property(e => e.Type).HasMaxLength(255);
                etb.Property(e => e.Group).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<ShiftReadModel>(etb =>
            {
                etb.ToTable("Weaving_ShiftDocuments");
                etb.HasKey(e => e.Identity);
                etb.Property(e => e.Name).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

                modelBuilder.Entity<DailyOperationLoomDetail>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationLoomDetails");
                etb.HasKey(e => e.Identity);
                
                etb.Property(e => e.Information).HasMaxLength(2000);
                etb.Property(e => e.DetailStatus).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationMachineLoomReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationLoomDocuments");
                etb.HasKey(e => e.Identity);

                etb.HasMany(e => e.DailyOperationLoomDetails)
                    .WithOne(e => e.DailyOperationLoomDocument)
                    .HasForeignKey(e => e.DailyOperationLoomDocumentId);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MachinesPlanningReadModel>(etb =>
            {
                etb.ToTable("Weaving_MachinesPlanningDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.Area).HasMaxLength(255);
                etb.Property(e => e.Blok).HasMaxLength(255);
                etb.Property(e => e.BlokKaizen).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MachineTypeReadModel>(etb =>
            {
                etb.ToTable("Weaving_MachineTypeDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.TypeName).HasMaxLength(255);
                etb.Property(e => e.MachineUnit).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MachineDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_MachineDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.MachineNumber).HasMaxLength(255);
                etb.Property(e => e.Location).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<EstimationProduct>(etb =>
            {
                etb.ToTable("Weaving_EstimationDetails");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.OrderDocument).HasMaxLength(2000);
                etb.Property(e => e.ProductGrade).HasMaxLength(2000);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<EstimatedProductionDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_EstimationProductDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.Period).HasMaxLength(255);

                etb.HasMany(e => e.EstimationProducts)
                    .WithOne(e => e.EstimatedProductionDocument)
                    .HasForeignKey(e => e.EstimatedProductionDocumentId);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<YarnDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_YarnDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Code).HasMaxLength(255);
                etb.Property(p => p.Name).HasMaxLength(255);
                etb.Property(p => p.Tags).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<WeavingSupplierDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_SupplierDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Code).HasMaxLength(255);
                etb.Property(p => p.Name).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<YarnNumberDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_YarnNumberDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Code).HasMaxLength(255);
                etb.Property(p => p.Number).HasMaxLength(255);
                etb.Property(p => p.Description).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<ConstructionDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_ConstructionDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.ConstructionNumber).HasMaxLength(255);
                etb.Property(p => p.WovenType).HasMaxLength(255);
                etb.Property(p => p.WarpType).HasMaxLength(255);
                etb.Property(p => p.WeftType).HasMaxLength(255);
                etb.Property(p => p.ListOfWarp).HasMaxLength(20000);
                etb.Property(p => p.ListOfWeft).HasMaxLength(20000);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MaterialTypeReadModel>(etb =>
            {
                etb.ToTable("Weaving_MaterialTypeDocument");
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

                etb.Property(p => p.ConstructionId).HasMaxLength(255);
                etb.Property(p => p.Period).HasMaxLength(255);
                etb.Property(p => p.WarpComposition).HasMaxLength(255);
                etb.Property(p => p.WeftComposition).HasMaxLength(255);
                etb.Property(p => p.OrderStatus).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
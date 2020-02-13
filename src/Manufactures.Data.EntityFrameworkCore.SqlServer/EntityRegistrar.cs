// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Manufactures.Domain.FabricConstructions.ReadModels;
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
using Manufactures.Domain.MachinesPlanning.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.Shifts.ReadModels;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Movements.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.StockCard.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using Manufactures.Domain.BeamStockMonitoring.ReadModels;
using Manufactures.Domain.BrokenCauses.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Loom;

namespace Manufactures.Data.EntityFrameworkCore
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<WarpingBrokenCauseReadModel>(etb =>
            {
                etb.ToTable("Weaving_WarpingBrokenCauseDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.WarpingBrokenCauseName).HasMaxLength(255);
                etb.Property(e => e.Information).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationLoomBeamProductReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationLoomBeamProducts");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationLoomBeamHistoryReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationLoomHistories");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationLoomReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationLoomDocuments");
                etb.HasKey(e => e.Identity);


                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<FabricDefectReadModel>(etb =>
            {
                etb.ToTable("Weaving_FabricDefectDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(e => e.DefectCode).HasMaxLength(255);
                etb.Property(e => e.DefectType).HasMaxLength(255);
                etb.Property(e => e.DefectCategory).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<BeamStockMonitoringReadModel>(etb =>
            {
                etb.ToTable("Weaving_BeamStockMonitoringDocuments");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationReachingHistoryReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationReachingHistories");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationReachingReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationReachingDocuments");
                etb.HasKey(e => e.Identity);


                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<StockCardReadModel>(etb =>
            {
                etb.ToTable("Weaving_StockCardDocuments");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationWarpingBrokenCauseReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationWarpingBrokenCauses");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationWarpingBeamProductReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationWarpingBeamProducts");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationWarpingHistoryReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationWarpingHistories");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationWarpingDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationWarpingDocuments");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MovementReadModel>(etb =>
            {
                etb.ToTable("Weaving_MovementDocuments");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<BeamReadModel>(etb =>
            {
                etb.ToTable("Weaving_BeamDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Number).HasMaxLength(255);
                etb.Property(p => p.Type).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationSizingBeamsWarpingReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingBeamsWarping");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationSizingHistoryReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingHistories");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationSizingBeamProductReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingBeamProducts");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<DailyOperationSizingDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_DailyOperationSizingDocuments");
                etb.HasKey(e => e.Identity);

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

            modelBuilder.Entity<EstimatedProductionDetailReadModel>(etb =>
            {
                etb.ToTable("Weaving_EstimationProductDetails");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<EstimatedProductionDocumentReadModel>(etb =>
            {
                etb.ToTable("Weaving_EstimationProductDocuments");
                etb.HasKey(e => e.Identity);

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

            modelBuilder.Entity<ConstructionYarnDetailReadModel>(etb =>
            {
                etb.ToTable("Weaving_ConstructionYarnDetails");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Information).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<FabricConstructionReadModel>(etb =>
            {
                etb.ToTable("Weaving_ConstructionDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.ConstructionNumber).HasMaxLength(255);
                etb.Property(p => p.WovenType).HasMaxLength(255);
                etb.Property(p => p.WarpType).HasMaxLength(255);
                etb.Property(p => p.WeftType).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<MaterialTypeReadModel>(etb =>
            {
                etb.ToTable("Weaving_MaterialTypeDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.Code).HasMaxLength(255);
                etb.Property(p => p.Name).HasMaxLength(255);
                etb.Property(p => p.Description).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<OrderReadModel>(etb =>
            {
                etb.ToTable("Weaving_OrderDocuments");
                etb.HasKey(e => e.Identity);

                etb.Property(p => p.ConstructionDocumentId).HasMaxLength(255);
                etb.Property(p => p.Period).HasMaxLength(255);
                etb.Property(p => p.OrderStatus).HasMaxLength(255);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
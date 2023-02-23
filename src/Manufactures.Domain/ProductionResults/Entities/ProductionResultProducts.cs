using Infrastructure.Domain;
using Infrastructure.Domain.Events;
using Manufactures.Domain.ProductionResults.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.ProductionResults.Entities
{
    public class ProductionResultProducts : EntityBase<ProductionResultProducts>
    {
        public Guid MachineDocumentId { get; private set; }

        public double Production { get; private set; }

        public double SCMPX { get; private set; }

        public double Efficiency { get; private set; }

        public double WeftBrokenThreads { get; private set; }

        public double WarpBrokenThreads { get; private set; }

        public double BrokenThreadsTotal { get; private set; }

        public TimeSpan LostTime { get; private set; }

        public TimeSpan ProductionTime { get; private set; }

        public Guid ProductionResultDocumentId { get; set; }

        public ProductionResultReadModel ProductionResultDocument { get; set; }

        public ProductionResultProducts(Guid identity) : base(identity)
        {
        }

        public ProductionResultProducts(Guid identity,
                                        MachineId machineDocumentId, 
                                        double production, 
                                        double scmpx, 
                                        double efficiency, 
                                        double weftBrokenThreads, 
                                        double warpBrokenThreads, 
                                        double brokenThreadsTotal, 
                                        TimeSpan lostTime, 
                                        TimeSpan productionTime) : base(identity)
        {
            Production = production;
            SCMPX = scmpx;
            Efficiency = efficiency;
            WeftBrokenThreads = weftBrokenThreads;
            WarpBrokenThreads = warpBrokenThreads;
            BrokenThreadsTotal = brokenThreadsTotal;
            LostTime = lostTime;
            ProductionTime = productionTime;
        }

        public void SetMachineId(MachineId machineDocumentId)
        {
            MachineDocumentId = machineDocumentId.Value;
            MarkModified();
        }

        public void SetProduction(double production)
        {
            Production = production;
            MarkModified();
        }

        public void SetSCMPX(double scmpx)
        {
            SCMPX = scmpx;
            MarkModified();
        }

        public void SetEfficiency(double efficiency)
        {
            Efficiency = efficiency;
            MarkModified();
        }

        public void SetWeftBrokenThreads(double weftBrokenThreads)
        {
            WeftBrokenThreads = weftBrokenThreads;
            MarkModified();
        }

        public void SetWarpBrokenThreads(double warpBrokenThreads)
        {
            WarpBrokenThreads = warpBrokenThreads;
            MarkModified();
        }

        public void SetBrokenThreadsTotal(double brokenThreadsTotal)
        {
            BrokenThreadsTotal = brokenThreadsTotal;
            MarkModified();
        }

        protected override ProductionResultProducts GetEntity()
        {
            return this;
        }

        protected override void MarkRemoved()
        {
            DeletedBy = "System";
            Deleted = true;
            DeletedDate = DateTimeOffset.UtcNow;

            if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<ProductionResultProducts>))
                this.AddDomainEvent(new OnEntityDeleted<ProductionResultProducts>(GetEntity()));

            // clear updated events
            if (this.DomainEvents.Any(o => o is OnEntityUpdated<ProductionResultProducts>))
            {
                this.DomainEvents.Where(o => o is OnEntityUpdated<ProductionResultProducts>)
                    .ToList()
                    .ForEach(o => this.RemoveDomainEvent(o));
            }
        }
    }
}

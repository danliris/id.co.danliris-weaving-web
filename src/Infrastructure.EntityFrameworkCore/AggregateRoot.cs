using Infrastructure.Domain.Events;
using Infrastructure.Domain.ReadModels;
using Moonlay.Domain;
using System;
using System.Linq;

namespace Infrastructure.Domain
{
    public abstract class AggregateRoot<TAggregateRoot, TReadModel> : Entity, IAggregateRoot where TReadModel : ReadModelBase
    {
        protected AggregateRoot(Guid identity)
        {
            Identity = identity;

            this.MarkTransient();
        }

        protected AggregateRoot(TReadModel readModel)
        {
            Identity = readModel.Identity;

            ReadModel = readModel;
        }

        protected TReadModel ReadModel { get; set; }

        public TReadModel GetReadModel()
        {
            if (this.IsTransient())
            {
                if (ReadModel.DomainEvents == null || !ReadModel.DomainEvents.Any(o => o is OnEntityCreated<TAggregateRoot>))
                    ReadModel.AddDomainEvent(new OnEntityCreated<TAggregateRoot>(GetEntity()));
            }

            return ReadModel;
        }

        public IAuditTrail AuditTrail => ReadModel;

        public ISoftDelete SoftDelete => ReadModel;

        private bool _modified = false;

        public virtual void MarkModified()
        {
            _modified = true;

            if (ReadModel.DomainEvents == null || !ReadModel.DomainEvents.Any(o => o is OnEntityUpdated<TAggregateRoot>))
                ReadModel.AddDomainEvent(new OnEntityUpdated<TAggregateRoot>(GetEntity()));
        }

        protected abstract TAggregateRoot GetEntity();

        public virtual bool IsModified()
        {
            return _modified;
        }

        public virtual void MarkRemoved()
        {
            ReadModel.AddDomainEvent(new OnEntityDeleted<TAggregateRoot>(GetEntity()));
        }
    }
}

using Infrastructure.Domain.Events;
using Infrastructure.Domain.ReadModels;
using Moonlay;
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

        protected abstract TAggregateRoot GetEntity();

        public IAuditTrail AuditTrail => ReadModel;

        public ISoftDelete SoftDelete => ReadModel;

        private bool _modified = false;

        public void MarkModified()
        {
            if (!this.IsTransient())
            {
                Validator.ThrowWhenTrue(() => IsDeleted(), "Entity cannot be modified, it was set as Deleted Entity");

                _modified = true;
                if (ReadModel.DomainEvents == null || !ReadModel.DomainEvents.Any(o => o is OnEntityUpdated<TAggregateRoot>))
                    ReadModel.AddDomainEvent(new OnEntityUpdated<TAggregateRoot>(GetEntity()));
            }
        }

        public virtual bool IsModified()
        {
            return _modified;
        }

        private bool _deleted = false;

        public void MarkRemoved()
        {
            if (!this.IsTransient())
            {
                _deleted = true;

                if (ReadModel.DomainEvents == null || !ReadModel.DomainEvents.Any(o => o is OnEntityDeleted<TAggregateRoot>))
                    ReadModel.AddDomainEvent(new OnEntityDeleted<TAggregateRoot>(GetEntity()));

                // clear updated events
                if (ReadModel.DomainEvents.Any(o => o is OnEntityUpdated<TAggregateRoot>))
                {
                    ReadModel.DomainEvents.Where(o => o is OnEntityUpdated<TAggregateRoot>)
                        .ToList()
                        .ForEach(o => ReadModel.RemoveDomainEvent(o));
                }
            }
        }

        public virtual bool IsDeleted()
        {
            return _deleted;
        }
    }
}

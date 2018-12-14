using Infrastructure.Domain.Events;
using Infrastructure.Domain.ReadModels;
using Moonlay.Domain;
using System;
using System.Linq;

namespace Infrastructure.Domain
{
    public abstract class EntityBase<TEntity> : ReadModelBase
    {
        protected EntityBase(Guid identity) : base(identity)
        {
            this.MarkTransient();

            this.AddDomainEvent(new OnEntityCreated<TEntity>(GetEntity()));
        }

        public IAuditTrail AuditTrail => this;

        public ISoftDelete SoftDelete => this;

        private bool _modified = false;

        public void MarkModified()
        {
            if (!this.IsTransient())
            {
                Moonlay.Validator.ThrowWhenTrue(() => IsDeleted(), "Entity cannot be modified, it was set as Deleted Entity");

                _modified = true;
                if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityUpdated<TEntity>))
                    this.AddDomainEvent(new OnEntityUpdated<TEntity>(GetEntity()));
            }
        }

        protected abstract TEntity GetEntity();

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

                if (this.DomainEvents == null || !this.DomainEvents.Any(o => o is OnEntityDeleted<TEntity>))
                    this.AddDomainEvent(new OnEntityDeleted<TEntity>(GetEntity()));

                // clear updated events
                if (this.DomainEvents.Any(o => o is OnEntityUpdated<TEntity>))
                {
                    this.DomainEvents.Where(o => o is OnEntityUpdated<TEntity>)
                        .ToList()
                        .ForEach(o => this.RemoveDomainEvent(o));
                }
            }
        }

        public virtual bool IsDeleted()
        {
            return _deleted;
        }
    }
}

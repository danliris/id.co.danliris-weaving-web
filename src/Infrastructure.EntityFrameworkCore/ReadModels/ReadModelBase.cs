using ExtCore.Data.Entities.Abstractions;
using Infrastructure.Domain.Events;
using Moonlay.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infrastructure.Domain.ReadModels
{
    public abstract class ReadModelBase : ReadModel, IEntity, IAuditTrail, ISoftDelete
    {
        protected ReadModelBase(Guid identity)
        {
            Identity = identity;
        }

        #region IAuditTrail ISoftDelete
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public bool? Deleted { get; set; }

        public DateTimeOffset? DeletedDate { get; set; }

        public string DeletedBy { get; set; }
        #endregion
    }

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

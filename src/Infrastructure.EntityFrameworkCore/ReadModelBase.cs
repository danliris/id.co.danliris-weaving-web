using ExtCore.Data.Entities.Abstractions;
using Infrastructure.Domain.Events;
using Moonlay.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

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

    //public static class ReadModelBaseExt
    //{
    //    public static void AddDomainEvent<T, TProperty>(this T model, Expression<Func<T, TProperty>> expression) where T : ReadModelBase
    //    {
    //        model.AddDomainEvent(new OnEntityUpdated<T, TProperty>(expression));
    //    }
    //}
}

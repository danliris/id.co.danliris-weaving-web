using ExtCore.Data.EntityFramework;
using ExtCore.Data.EntityFramework.SqlServer;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DanLiris.Admin.Web
{
    public class DanlirisStorageContext : StorageContext
    {
        private readonly IWorkContext _workContext;

        public DanlirisStorageContext(IOptions<StorageContextOptions> options, IWorkContext workContext) : base(options)
        {
            _workContext = workContext;
        }

        private void AuditTrack()
        {
            if (_workContext == null) return;

            var now = DateTime.Now;

            var addedAuditedEntities = ChangeTracker.Entries<DanLirisReadModel>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<DanLirisReadModel>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);

            if (!modifiedAuditedEntities.Any() && !addedAuditedEntities.Any())
                return;

            var currentUser = _workContext.CurrentUser ?? "System";

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedBy = currentUser;
                added.CreatedDate = now;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.ModifiedBy = currentUser;
                modified.ModifiedDate = now;
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditTrack();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            AuditTrack();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditTrack();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AuditTrack();

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
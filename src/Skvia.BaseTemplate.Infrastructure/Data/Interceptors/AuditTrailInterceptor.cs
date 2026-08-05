using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Application.Features.Auth.DTOs;
using Skvia.BaseTemplate.Domain.Auditing;
using Skvia.BaseTemplate.Domain.Common;

namespace Skvia.BaseTemplate.Infrastructure.Data.Interceptors;

public class AuditTrailInterceptor(ICurrentUserProvider currentUserProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        CreateAuditLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        CreateAuditLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void CreateAuditLogs(DbContext? context)
    {
        if (context == null) return;

        Guid? userId = null;

        try
        {
            CurrentUserResponse currentUser = currentUserProvider.GetCurrentUser();
            userId = currentUser?.Id;
        }
        catch (InvalidOperationException)
        {
            userId = Guid.Empty;
        }

        var auditEntries = new List<AuditLog>();
        var entries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is AuditLog) continue;

            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();
            var affectedColumns = new List<string>();

            string action = entry.State.ToString();

            if (entry.Entity is IArchivable archivableEntry && entry.State == EntityState.Modified)
            {
                var isArchivedProperty = entry.Property(nameof(IArchivable.IsArchived));
                if (isArchivedProperty.IsModified)
                {
                    action = archivableEntry.IsArchived ? "Archived" : "Unarchived";
                }
            }

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsShadowProperty()) continue;

                string propertyName = property.Metadata.Name;

                switch (entry.State)
                {
                    case EntityState.Added:
                        newValues[propertyName] = property.CurrentValue;
                        affectedColumns.Add(propertyName);
                        break;

                    case EntityState.Deleted:
                        oldValues[propertyName] = property.OriginalValue;
                        affectedColumns.Add(propertyName);
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            oldValues[propertyName] = property.OriginalValue;
                            newValues[propertyName] = property.CurrentValue;
                            affectedColumns.Add(propertyName);
                        }
                        break;
                }
            }

            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityName = entry.Entity.GetType().Name,
                EntityId = entry.Property("Id").CurrentValue?.ToString(),
                OldValuesJson = oldValues.Count > 0 ? JsonSerializer.Serialize(oldValues) : null,
                NewValuesJson = newValues.Count > 0 ? JsonSerializer.Serialize(newValues) : null,
                AffectedColumnsJson = affectedColumns.Count > 0 ? JsonSerializer.Serialize(affectedColumns) : null,
                Timestamp = DateTimeOffset.UtcNow
            };

            auditEntries.Add(auditLog);
        }

        if (auditEntries.Count > 0)
        {
            context.Set<AuditLog>().AddRange(auditEntries);
        }
    }
}

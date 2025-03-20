using Microsoft.EntityFrameworkCore.Diagnostics;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace StudentManagement.DAL.Data
{
    public class AuditInterceptor: SaveChangesInterceptor
    {
        private readonly List<AuditEntry> _auditEntries;
        public AuditInterceptor(List<AuditEntry> auditEntries)
        {
            _auditEntries = auditEntries;
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
        {
            if(eventData.Context is null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            var startTime = DateTime.Now;
            var auditEntries = eventData.Context.ChangeTracker.Entries()
                .Where(x => x.Entity is not AuditEntry && x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .Select(x => new AuditEntry
                {
                    Id = Guid.NewGuid(),
                    StartTimeUtc = startTime,
                    Metadata = x.DebugView.LongView
                }).ToList();
            if(auditEntries.Count == 0)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            _auditEntries.AddRange(auditEntries);
            return await base.SavingChangesAsync(eventData,result,  cancellationToken);
        }

        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
        {

            if(eventData.Context is null)
            {
                return await base.SavedChangesAsync(eventData, result, cancellationToken);
            }
            var endTime = DateTime.Now;
            foreach (var auditEntry in _auditEntries)
            {
                auditEntry.EndTimeUtc = endTime;
                auditEntry.IsSuccess = true;
            }

            if(_auditEntries.Count > 0)
            {
                eventData.Context.AddRange(_auditEntries);
                _auditEntries.Clear();
                await eventData.Context.SaveChangesAsync(cancellationToken);
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public override async Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context is null)
            {
                return;
            }

            var endTime = DateTime.Now;

            foreach (var auditEntry in _auditEntries)
            {
                auditEntry.EndTimeUtc = endTime;
                auditEntry.IsSuccess = false;
                auditEntry.ErrorMessage = eventData.Exception.Message;
                if (eventData.Exception.InnerException is not null)
                {
                    auditEntry.ErrorMessage += " | Inner Exception: " + eventData.Exception.InnerException.Message;
                }
            }

            if (_auditEntries.Count > 0)
            {
                // Tạo một DbContext mới không nằm trong transaction bị lỗi
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                     .UseSqlServer(eventData.Context.Database.GetDbConnection())
                     .Options;
                using var auditContext = new ApplicationDbContext(options, _auditEntries);
                auditContext.AddRange(_auditEntries);
                _auditEntries.Clear();
                await auditContext.SaveChangesAsync(cancellationToken);
            }

            await base.SaveChangesFailedAsync(eventData, cancellationToken);
        }


    }
}

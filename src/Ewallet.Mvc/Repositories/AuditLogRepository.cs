using Ewallet.Mvc.Data;
using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken ct);
}


public sealed class AuditLogRepository(AppDbContext db) : IAuditLogRepository
{
    public async Task AddAsync(AuditLog log, CancellationToken ct) => await db.AuditLogs.AddAsync(log,ct);
    
}
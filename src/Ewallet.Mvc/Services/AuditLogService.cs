using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;

namespace Ewallet.Mvc.Services;


public interface IAuditLogService
{
    Task<List<AuditLog>> GetAllAsync(CancellationToken ct);
}

public sealed class AuditLogService(IAuditLogRepository repository) : IAuditLogService
{
    public Task<List<AuditLog>> GetAllAsync(CancellationToken ct) => repository.GetAllAsync(ct);
}
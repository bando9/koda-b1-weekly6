using Ewallet.Mvc.Data;
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken ct);

    Task<List<AuditLog>> GetAllAsync(CancellationToken ct);
}


public sealed class AuditLogRepository(AppDbContext db) : IAuditLogRepository
{
    private readonly AppDbContext _db = db;
    public async Task AddAsync(AuditLog log, CancellationToken ct) => await _db.AuditLogs.AddAsync(log,ct);

    public async Task<List<AuditLog>> GetAllAsync(CancellationToken ct) => await _db.AuditLogs.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
}
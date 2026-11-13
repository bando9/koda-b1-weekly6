using Ewallet.Mvc.Data;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Repository;

public sealed class UserRepository(AppDbContext db) : IUserRepository
{
    private readonly AppDbContext _db = db;

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == id,cancellationToken);
    }
}
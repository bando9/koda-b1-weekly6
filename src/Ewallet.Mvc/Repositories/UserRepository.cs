using Ewallet.Mvc.Data;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Repository;

public sealed class UserRepository(AppDbContext db) : IUserRepository
{
    private readonly AppDbContext _db = db;

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == id,cancellationToken);
    }

    public async Task<long> CreateAsync(User user, CancellationToken ct){
        await _db.Users.AddAsync(user,ct);
        await _db.SaveChangesAsync(ct);
        return user.Id;        
    }

    public async Task CreateWalletAsync(Wallet wallet, CancellationToken ct)
    {
        await _db.Wallets.AddAsync(wallet, ct);
        await _db.SaveChangesAsync(ct);

    }

    // public async Task<User?> GetConflictUserAsync(string username, string email, string phone, CancellationToken ct)
    // {
    //     return await _db.Users
    //         .AsNoTracking()
    //         .FirstOrDefaultAsync(u => u.Username == username || u.Email 
    //         == email || u.Phone == phone, ct);
    // }

    public async Task<User?> GetConflictUserAsync(string username,string email, string phone, CancellationToken ct)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Username == username || u.Email == email || u.Phone == phone,ct);
    }

    
}
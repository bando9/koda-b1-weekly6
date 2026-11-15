using Ewallet.Mvc.Data;
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Repositories;

public sealed class WalletRepository(AppDbContext db) : IWalletRepository
{
    private AppDbContext _db = db;


    public async Task<WalletResponse?> GetByIdAsync(long id, CancellationToken ct)
    {
        var wallet = await _db.Wallets.FirstOrDefaultAsync(w=>w.Id == id, ct);

        return wallet is null ? null : WalletResponse.FromModel(wallet);
    }

    // public async Task
}
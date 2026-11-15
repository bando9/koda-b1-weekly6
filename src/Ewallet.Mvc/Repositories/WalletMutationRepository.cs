using Ewallet.Mvc.Data;
using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.Repositories;

public interface IWalletMutationRepository
{
    Task AddAsync(WalletMutation mutation,CancellationToken ct);
}

public sealed class WalletMutationRepository(AppDbContext db) : IWalletMutationRepository
{
    private readonly AppDbContext _db=db;
    public async Task AddAsync(WalletMutation mutation, CancellationToken ct) => await _db.WalletMutations.AddAsync(mutation,ct);

}
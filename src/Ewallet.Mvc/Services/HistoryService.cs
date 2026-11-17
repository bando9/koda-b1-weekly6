using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Repositories;

namespace Ewallet.Mvc.Services;

public interface IHistoryService
{
    Task<PagedResult<MutationResponse>> GetMutationsAsync(long walletId, MutationQuery query, CancellationToken ct);
}

public sealed class HistoryService(
    IWalletRepository walletRepository,
    IWalletMutationRepository walletMutationRepository
) : IHistoryService
{
    public async Task<PagedResult<MutationResponse>> GetMutationsAsync(long walletId, MutationQuery query, CancellationToken ct)
    {
        if(!await walletRepository.ExistAsync(walletId,ct)) throw new InvalidOperationException($"wallet dg id {walletId} tidak ditemukan");

        return await walletMutationRepository.GetHistoryAsync(walletId,query,ct); 
    }
}
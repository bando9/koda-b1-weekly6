using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.Repositories;

public interface IWalletRepository
{
    Task<WalletResponse?> GetByIdAsync(long id, CancellationToken ct);

}
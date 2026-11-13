using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.Repositories;

public interface IUserRepository
{
    // Create User + Wallet
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);

    // Task<int?> TryAddWithWalletAsync(User user, Wallet wallet, CancellationToken cancellationToken);

    // Task<bool> TryUpdateAsync(User user, CancellationToken cancellationToken);




};
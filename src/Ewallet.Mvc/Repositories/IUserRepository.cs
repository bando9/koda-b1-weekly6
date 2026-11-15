using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);
    
    Task<long> CreateAsync(User user, CancellationToken ct);

    Task CreateWalletAsync(Wallet wallet, CancellationToken ct);




    Task<User?> GetConflictUserAsync(string username,string email, string phone, CancellationToken ct);

};
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Repositories;
using Ewallet.Mvc.Repository;

namespace Ewallet.Mvc.Services;

public sealed class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository=repository;

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repository.GetAllAsync(ct);

        return users.Select(UserResponse.FromModel).ToArray();
    }

    public Task<UserResponse> GetByIdAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
using Ewallet.Mvc.DTOs;

namespace Ewallet.Mvc.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken ct);

    Task<UserResponse> GetByIdAsync(CancellationToken ct);
}
using System.Security.Cryptography;
using Ewallet.Mvc.Data;
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;
using Ewallet.Mvc.Repository;
using Microsoft.Data.SqlClient;

namespace Ewallet.Mvc.Services;

public sealed class UserService(IUserRepository repository,AppDbContext context) : IUserService
{
    private readonly IUserRepository _repository=repository;
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repository.GetAllAsync(ct);

        return users.Select(UserResponse.FromModel).ToArray();
    }

    public async Task<UserResponse?> GetByIdAsync(long id, CancellationToken ct)
    {
        
        if (id < 0)
        {
            return null!;
        }

        var user = await _repository.GetByIdAsync(id, ct);

        return user is null ? null : UserResponse.FromModel(user);
    }

    public async Task<UserResponse?> CreateWithWalletAsync(UserRequest request,CancellationToken ct)
    {

        var existingUser = await  _repository.GetConflictUserAsync(request.Username,request.Email,request.Phone,ct);

        if(existingUser is not null)
        {
            if(existingUser.Username == request.Username)
            {
                throw new InvalidOperationException("username already exist");
            }
            if(existingUser.Email == request.Email)
            {
                throw new InvalidOperationException("email already exist");
            }
            if(existingUser.Phone == request.Phone)
            {
                throw new InvalidOperationException("phone already exist");
            }
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            var user = new User(0, request.Username,request.FullName,request.Email,request.Phone,request?.PasswordHash,UserStatusEnum.ACTIVE);

            long userId = await _repository.CreateAsync(user,ct);

            var wallet = new Wallet(0,GenerateWalletNumber(),userId);
            
            await _repository.CreateWalletAsync(wallet,ct);

            await transaction.CommitAsync(ct);

            return UserResponse.FromModel(user);

        }
        catch (SqlException e) when (e.Number is 2601 or 2627)
        {
            await transaction.RollbackAsync(ct);
            return null;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }


    // public async Task<WalletResponse?> CreateWithWalletAsync(User user, Wallet wallet, CancellationToken ct)
    // {        
    //     var wallet = new Wallet(0,GenerateWalletNumber(), 100,0);
    //     var created = await _repository.TryAddWithWalletAsync(wallet, ct);
    //     return created ? WalletResponse.FromModel(wallet) : null;        
    // }


    private static string GenerateWalletNumber()
    {
        return string.Concat(Enumerable.Range(0, 12)
            .Select(_ => RandomNumberGenerator.GetInt32(0, 10)));
    }
    

}
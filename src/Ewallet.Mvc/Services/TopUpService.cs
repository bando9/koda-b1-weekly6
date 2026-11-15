
using System.Security.Cryptography;
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;

namespace Ewallet.Mvc.Services;

public interface ITopUpService
{
    Task<TopUpResponse> CreateAsync(CreateTopUpRequest request, CancellationToken ct);
    Task<TopUpResponse> GetByIdAsync(long topUpRequestId, CancellationToken ct);
    Task<TopUpResponse> ConfirmAsync(long topUpRequestId, CancellationToken ct);
}

public sealed class TopUpService (
    IWalletRepository walletRepository,
    IPaymentMethodRepository paymentMethodRepository,
    ITransactionRepository transactionRepository,
    ITopUpRepository topUpRepository,
    IWalletMutationRepository walletMutationRepository,
    IAuditLogRepository auditLogRepository,
    TimeProvider time

): ITopUpService
{
    private static readonly TimeSpan RequestLifeTime = TimeSpan.FromHours(24);
    public Task<TopUpResponse> ConfirmAsync(long topUpRequestId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<TopUpResponse> CreateAsync(CreateTopUpRequest request, CancellationToken ct)
    {
        // if(decimal.Round(request.Amount,2) != request.Amount)
        // {
        //     throw new InvalidOperationException("Nomimal maksimal 2 angka dibelakang koma");
        // }

        var wallet = await walletRepository.GetByIdAsync(request.WalletId,ct) ?? throw new InvalidOperationException("wallet tidak ditemukan");

        WalletStatusEnum active = WalletStatusEnum.ACTIVE;
        if (wallet.Status != active)
        {
            throw new InvalidOperationException("wallet tidak berstatus ACTIVE");
        }

        var method = await paymentMethodRepository.GetByIdAsync(request.PaymentMethodId,ct) ?? throw new InvalidOperationException("Metode pembayaran blm tersedia");
        if(!method.IsActive) throw new InvalidOperationException("metode pembayaran tidak aktif");

        var now = time.GetUtcNow().UtcDateTime;

        var transaction = Transaction.CreateTopUp(

          referenceNo: GenerateNumberRange(12),
          receiverWalletId: wallet.Id,
          amount: request.Amount,
          description: $"Top up via {method.Name}",
          nowUtc: now        
        );

        var topUp = TopUpRequest.Create(
            transaction:transaction,
            walletId:wallet.Id,
            paymentMethodId: method.Id,
            amount: request.Amount,
            providerReference: $"SIM-{method.Code}-{GenerateNumberRange(12)}"
            nowUtc:now,
            expiresAtUtc:now.Add(RequestLifeTime)
        );



    }

    public Task<TopUpResponse> GetByIdAsync(long topUpRequestId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    private static string GenerateNumberRange(int number)
    {
        return string.Concat(Enumerable.Range(0, 100)
            .Select(_ => RandomNumberGenerator.GetInt32(0, number)));
    }
}













// using System.Security.Cryptography;
// using Ewallet.Mvc.Data;
// using Ewallet.Mvc.DTOs;
// using Ewallet.Mvc.Models;
// using Ewallet.Mvc.Repositories;
// using Microsoft.EntityFrameworkCore;

// namespace Ewallet.Mvc.Services;

// public sealed class TopUpService(AppDbContext db, IWalletRepository walletrepo,IPaymentMethodRepository paymentmethodrepo,ITopUpRepository topuprepo) : ITopUpService
// {
//     private readonly AppDbContext _db = db;
//     private readonly IWalletRepository _walletrepo = walletrepo;
//     private readonly IPaymentMethodRepository _paymentmethodrepo = paymentmethodrepo;
//     private readonly ITopUpRepository _topuprepo = topuprepo;
//     public Task<TopUpResponse> ConfirmAsync(long TopUpRequestId, CancellationToken ct)
//     {
//         throw new NotImplementedException();
//     }

//     // public async Task<TopUpResponse> CreateAsync(CreateTopUpRequest request, CancellationToken ct)
//     // {
//     //     if(request.Amount < 0)
//     //     {
//     //         throw new InvalidOperationException("Maaf angka nominal topup tidak boleh kurang dari 0");
//     //     }

//     //     var existingWallet = await _walletrepo.GetByIdAsync(request.WalletId, ct);
//     //     var frozenWallet = WalletStatusEnum.FROZEN;
//     //     var closedWallet = WalletStatusEnum.CLOSED;

//     //     if(existingWallet is null || existingWallet.Status == frozenWallet || existingWallet.Status == closedWallet)
//     //     {
//     //         throw new InvalidOperationException("Maaf wallet tidak bisa digunakan atau tidak aktif.");
//     //     }

//     //     var existingPaymentMethod = await _paymentmethodrepo.GetByIdAsync(request.PaymentMethodId, ct);

//     //     if (!existingPaymentMethod.IsActive)
//     //     {
//     //         throw new InvalidOperationException("maaf metode payment sudah tidak aktif");
//     //     }


//     //     Transaction transaction = new(
//     //         0,
//     //         $"Ref-{GenerateNumberRange(8)}",
//     //         TransactionTypeEnum.TOPUP,
//     //         request.Amount,
//     //         TransactionStatusEnum.PENDING,
//     //         "tes descfrom topupservice",
//     //         DateTime.UtcNow,
//     //         null,
//     //         null,
//     //         request.WalletId
//     //     );

//     //     var topUpStatusPending = TopUpRequestStatusEnum.PENDING;
//     //     TopUpRequest topUpReq = new(
//     //         0,
//     //         request.Amount,
//     //         topUpStatusPending,
//     //         null,
//     //         DateTime.UtcNow,
//     //         null,
//     //         null
//     //     );

//     //     // AuditLog auditLog = new(0,"")

//     //     AuditLog auditLog = new(0,"TOPUP_REQUESTED","Transaction",$"{transaction.Id}",OutcomeEnum.SUCCESS,"tes from topup service",null,DateTime.UtcNow);

//     //     await _topuprepo.AddRequestAsync(request,auditLog,ct);

//     // }


//     private static string GenerateNumberRange(int number)
//     {
//         return string.Concat(Enumerable.Range(0, 100)
//             .Select(_ => RandomNumberGenerator.GetInt32(0, number)));
//     }

// }
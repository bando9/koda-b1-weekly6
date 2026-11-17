using System.Diagnostics;
using System.Security.Cryptography;
using Ewallet.Mvc.Data;
using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Mvc.Services;

public interface ITransferService
{
    Task<TransferResponse> CreateAsync(CreateTransferRequest request, CancellationToken ct);
}




public sealed class TransferService(
    IWalletRepository walletRepo,
    ITransactionRepository transactionRepo,
    IWalletMutationRepository walletMutationRepo,
    IAuditLogRepository auditLogRepo,
    TimeProvider time,
    AppDbContext db
    // ILogger
) : ITransferService
{
    private readonly AppDbContext _db=db;
    private sealed class Attempt
    {
        public long? ActoreUserId {get; set;}
    }
    public async Task<TransferResponse> CreateAsync(CreateTransferRequest request, CancellationToken ct)
    {
        var now = time.GetUtcNow().UtcDateTime;
        var correlationId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");
        long? actorUserId = null;

        await using var dbTx = await db.Database.BeginTransactionAsync(ct);

        try
        {
            var sender = await walletRepo.GetWalletByIdAsync(request.SenderWalletId, ct)
                ?? throw new InvalidOperationException("Wallet pengirim tidak ditemukan.");
            actorUserId = sender.UserId;

            var receiver = await walletRepo.GetByWalletNumberAsync(request.ReceiverWalletNumber.Trim(), ct)
                ?? throw new InvalidOperationException("Wallet penerima tidak ditemukan.");

            // if (sender.Id == receiver.Id)
            //     throw new InvalidOperationException("Wallet pengirim dan penerima tidak boleh sama.");
            // if (!sender.IsActive)
            //     throw new InvalidOperationException("Wallet pengirim tidak berstatus ACTIVE.");
            // if (!receiver.IsActive)
            //     throw new InvalidOperationException("Wallet penerima tidak berstatus ACTIVE.");
            if (sender.Balance < request.Amount)
                throw new InvalidOperationException("Saldo tidak mencukupi.");

            // 2. Potong & Tambah Saldo
            var senderBefore = sender.Balance;
            var receiverBefore = receiver.Balance;
            var amount = request.Amount;

            sender.Debit(amount);
            receiver.Credit(amount);

            var transaction = Transaction.CreateTransfer(
                referenceNo: $"TRF-{GenerateWalletNumber()}",
                senderWalletId: sender.Id,
                receiverWalletId: receiver.Id,
                amount: amount,
                description: string.IsNullOrWhiteSpace(request.Description)
                    ? $"Transfer ke {receiver.WalletNumber}"
                    : request.Description.Trim(),
                nowUtc: now);

            await transactionRepo.AddAsync(transaction, ct);

            await walletMutationRepo.AddAsync(WalletMutation.CreateOut(
                transaction: transaction, walletId: sender.Id, amount: amount,
                balanceBefore: senderBefore, balanceAfter: sender.Balance, nowUtc: now), ct);

            await walletMutationRepo.AddAsync(WalletMutation.CreateIn(
                transaction: transaction, walletId: receiver.Id, amount: amount,
                balanceBefore: receiverBefore, balanceAfter: receiver.Balance, nowUtc: now), ct);

            await db.SaveChangesAsync(ct);

            await auditLogRepo.AddAsync(AuditLog.Create(
                action: "TRANSFER_CREATED",
                entityType: "Transaction",
                entityId: transaction.Id.ToString(),
                outcome: OutcomeEnum.SUCCESS,
                description: "Transfer berhasil.",
                actorUserId: sender.UserId,
                transactionId: transaction.Id,
                correlationId: correlationId,
                nowUtc: now), ct);

            await db.SaveChangesAsync(ct);
            await dbTx.CommitAsync(ct);

            return new TransferResponse
            {
                TransactionId = transaction.Id,
                ReferenceNo = transaction.ReferenceNo,
                SenderWalletId = sender.Id,
                ReceiverWalletId = receiver.Id,
                ReceiverWalletNumber = receiver.WalletNumber,
                Amount = amount,
                Status = transaction.Status.ToString(),
                Description = transaction.Description,
                SenderBalanceAfter = sender.Balance,
                CompletedAtUtc = transaction.CompletedAtUtc!.Value
            };
        }
        catch (Exception ex) when (
            ex is InvalidOperationException
            or DbUpdateConcurrencyException
            or DbUpdateException { InnerException: SqlException { Number: 1205 } })
        {
            await dbTx.RollbackAsync(CancellationToken.None);
            db.ChangeTracker.Clear();

            var isBusinessRule = ex is InvalidOperationException;
            var reason = isBusinessRule
                ? ex.Message
                : "Transfer dibatalkan karena konflik transaksi bersamaan. Silakan coba lagi.";

            await auditLogRepo.AddAsync(AuditLog.Create(
                action: "TRANSFER_REJECTED",
                entityType: "Wallet",
                entityId: request.SenderWalletId.ToString(),
                outcome: isBusinessRule ? OutcomeEnum.REJECTED : OutcomeEnum.FAILED,
                description: reason,
                actorUserId: actorUserId,
                transactionId: null,
                correlationId: correlationId,
                nowUtc: now), CancellationToken.None);

            await db.SaveChangesAsync(CancellationToken.None);

            if (isBusinessRule) throw;
            throw new InvalidOperationException(reason);
        }
    }

    // public async Task<TransferResponse> CreateAsync(CreateTransferRequest request, CancellationToken ct)
    // {
    //     var attempt = new Attempt();

    //     try
    //     {
    //         return await ExecuteAsync(request,attempt,ct);
    //     }
    //     catch (Exception ex) when (
    //         ex is DbUpdateException or DbUpdateConcurrencyException {InnerException: SqlException {Number: 1205}}
    //     )
    //     {
    //         await dbTx
    //     }
    // }


    // private async Task<TransferResponse> ExecuteAsync (
    //     CreateTransferRequest request, Attempt attempt, CancellationToken ct
    // )
    // {
    //     var sender = await walletRepo.GetWalletByIdAsync(request.SenderWalletId,ct) ?? throw new InvalidOperationException("Wallet pengirim tidak ditemukan");
    //     attempt.ActoreUserId = sender.UserId;

    //     var receiver = await walletRepo.GetByWalletNumberAsync(request.ReceiverWalletNumber.Trim(),ct) ?? throw new InvalidOperationException("wallet penerima tidak ditemukan.");

    //     if(sender.Id == receiver.Id) throw new InvalidOperationException("wallet pengirim dan wallet penerima tidak boleh sama");
    //     var closedWallet = WalletStatusEnum.CLOSED;
    //     var frozenWallet = WalletStatusEnum.FROZEN;
    //     if(sender.Status == closedWallet || sender.Status == frozenWallet) throw new InvalidOperationException("wallet pengirim tidak aktif / sedang dibekukan.");
    //     if(receiver.Status == closedWallet || receiver.Status == frozenWallet) throw new InvalidOperationException("wallet penerima tidak aktif / sedang dibekukan.");

    //     var now = time.GetUtcNow().UtcDateTime;
    //     var amount = request.Amount;

    //     await using var dbTx = await _db.Database.BeginTransactionAsync(ct);

    //     var senderBefore = sender.Balance;
    //     var receiverBefore = receiver.Balance;

    //     sender.Debit(amount);
    //     receiver.Credit(amount);

    //     var transaction = Transaction.CreateTransfer(
    //          referenceNo: $"TRF-{GenerateWalletNumber()}",
    //         senderWalletId: sender.Id,
    //         receiverWalletId: receiver.Id,
    //         amount: amount,
    //         description: string.IsNullOrWhiteSpace(request.Description)
    //             ? $"Transfer ke {receiver.WalletNumber}"
    //             : request.Description.Trim(),
    //         nowUtc: now
    //     );

    //     await transactionRepo.AddAsync(transaction,ct);

    //     await walletMutationRepo.AddAsync(WalletMutation.CreateOut(
    //         transaction:transaction,
    //         walletId:sender.Id, 
    //         amount:amount,
    //         balanceBefore:senderBefore,
    //         balanceAfter:sender.Balance,
    //         nowUtc:now
    //     ), ct);

    //     await walletMutationRepo.AddAsync(WalletMutation.CreateIn(
    //         transaction: transaction,
    //         walletId: receiver.Id,
    //         amount: amount,
    //         balanceBefore: receiverBefore,
    //         balanceAfter: receiver.Balance,
    //         nowUtc: now), ct
    //     );

    //     await _db.SaveChangesAsync(ct);

    //     var correlationId = Activity.Current?.TraceId.ToString() 
    //                 ?? Guid.NewGuid().ToString("N");

    //     await auditLogRepo.AddAsync(AuditLog.Create(
    //         action: "TRANSFER_CREATED",
    //         entityType: "Transaction",
    //         entityId: transaction.Id.ToString(),
    //         outcome: OutcomeEnum.SUCCESS,
    //         description: "Transfer berhasil.",   // tanpa nominal/saldo: pesan aman
    //         actorUserId: sender.UserId,
    //         transactionId: transaction.Id,
    //         correlationId: correlationId,
    //         nowUtc: now), ct);

    //     await _db.SaveChangesAsync(ct);

    //     await dbTx.CommitAsync(ct);

    //     return new TransferResponse
    //     {
    //         TransactionId = transaction.Id,
    //         ReferenceNo = transaction.ReferenceNo,
    //         SenderWalletId = sender.Id,
    //         ReceiverWalletId = receiver.Id,
    //         ReceiverWalletNumber = receiver.WalletNumber,
    //         Amount = amount,
    //         Status = transaction.Status.ToString(),
    //         Description = transaction.Description,
    //         SenderBalanceAfter = sender.Balance,
    //         CompletedAtUtc = transaction.CompletedAtUtc!.Value
    //     };
    // }


    private static string GenerateWalletNumber()
    {
        return string.Concat(Enumerable.Range(0, 10)
            .Select(_ => RandomNumberGenerator.GetInt32(0, 10)));
    }
}
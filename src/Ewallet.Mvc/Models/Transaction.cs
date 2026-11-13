namespace Ewallet.Mvc.Models;

public sealed class Transaction
{
    public long Id {get; private set;}
    public string ReferenceNo {get; private set;} = string.Empty;
    public TransactionTypeEnum Type {get; private set;}
    public decimal Amount {get; private set;}
    public TransactionStatusEnum Status {get; private set;}
    public string? Description {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? CompletedAtUtc {get; private set;}

    // FK Wallet Sender
    public long? SenderWalletId {get; private set;} 
    public Wallet? SenderWallet {get; private set;}

    // FK Wallet Receiver
    public long ReceiverWalletId {get; private set;} 
    public Wallet ReceiverWallet {get; private set;} = null!;

    // constraint 
    public TopUpRequest? TopUpRequest {get; private set;}
    public ICollection<WalletMutation> WalletMutations {get; private set;} = new List<WalletMutation>();
    public ICollection<AuditLog> AuditLogs {get; private set;} = new List<AuditLog>();

    public Transaction(long id, string referenceNo, TransactionTypeEnum type, decimal amount, TransactionStatusEnum status, string? description, DateTime createdAtUtc, DateTime? completedAtUtc, long? senderWalletId, long receiverWalletId)
    {
        Id = id;
        ReferenceNo = referenceNo;
        Type = type;
        Amount = amount;
        Status = status;
        Description = description;
        CreatedAtUtc = createdAtUtc;
        CompletedAtUtc = completedAtUtc;
        SenderWalletId = senderWalletId;
        ReceiverWalletId = receiverWalletId;
    }

}

public enum TransactionTypeEnum {
    TOPUP, 
    TRANSFER
}

public enum TransactionStatusEnum {
    PENDING, 
    SUCCESS, 
    FAILED
}

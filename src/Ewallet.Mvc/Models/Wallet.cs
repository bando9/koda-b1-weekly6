namespace Ewallet.Mvc.Models;

public sealed class Wallet
{
    private Wallet() { }

    public long Id {get; private set;}
    public string WalletNumber	{get; private set;} = string.Empty;
    public decimal Balance	{get; private set;}
    public WalletStatusEnum Status	{get; private set;}
    public byte[] RowVersion {get; private set;} = null!;
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}

    // FK Users
    public long UserId	{get; private set;}
    public User User {get; private set;} = null!;

    // constraints
    public ICollection<Transaction> SentTransactions {get; private set;} = new List<Transaction>();
    public ICollection<Transaction> ReceivedTransactions  {get; private set;} = new List<Transaction>();
    public ICollection<WalletMutation> WalletMutations {get; private set;} = new List<WalletMutation>();
    public ICollection<TopUpRequest> TopUpRequests {get; private set;} = new List<TopUpRequest>();

    public Wallet(long id, string walletNumber, long userId,decimal balance = 0)
    {        
        Id=id;
        WalletNumber=walletNumber;
        Balance = balance >= 0 ? balance : throw new ArgumentException("Saldo gaboleh kurang dari 0");
        Status = WalletStatusEnum.ACTIVE;
        UserId=userId;
        CreatedAtUtc=DateTime.UtcNow;
        UpdatedAtUtc=null;
    }



}

public enum WalletStatusEnum {
    ACTIVE, 
    FROZEN, 
    CLOSED
}

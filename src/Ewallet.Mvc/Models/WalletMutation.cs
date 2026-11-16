namespace Ewallet.Mvc.Models;

public sealed class WalletMutation
{
    private WalletMutation() { } 

    public long Id	{get; private set;}
    public DirectionEnum Direction {get; private set;}
    public decimal Amount {get; private set;}
    public decimal BalanceBefore {get; private set;}
    public decimal BalanceAfter {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}
	
    // FK Transaction
    public long TransactionId {get; private set;}
    public Transaction Transaction {get; private set;} = null!;
    public long WalletId {get; private set;}
    public Wallet Wallet {get; private set;} = null!;


    public static WalletMutation CreateIn(
        long transactionId, long walletId, decimal amount, decimal balanceBefore, decimal balanceAfter, DateTime nowUtc
    )
    {
        if(balanceAfter != balanceBefore + amount) throw new ArgumentException("mutasi tidak konsisten dg saldo");

        return new WalletMutation
        {
            TransactionId=transactionId,
            WalletId=walletId,
            Direction=DirectionEnum.IN,
            Amount=amount,
            BalanceBefore=balanceBefore,
            BalanceAfter=balanceAfter,
            CreatedAtUtc=nowUtc
        };
    }
}

public enum DirectionEnum {
    IN, 
    OUT
}
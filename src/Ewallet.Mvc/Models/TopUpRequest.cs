namespace Ewallet.Mvc.Models;

public sealed class TopUpRequest
{
    public long Id	{get; private set;}
    public decimal Amount {get; private set;}
    public TopUpRequestStatusEnum Status {get; private set;}
    public string? ProviderReference	{get; private set;}
    public DateTime CreatedAtUtc	{get; private set;}
    public DateTime? ExpiresAtUtc	{get; private set;}
    public DateTime? PaidAtUtc	{get; private set;}

    // Foreign Key 
    public long TransactionId	{get; private set;}
    public Transaction Transaction	{get; private set;} = null!;
    public long WalletId {get; private set;}
    public Wallet Wallet {get; private set;} = null!;
    public int PaymentMethodId {get; private set;}
    public PaymentMethod PaymentMethod {get; private set;} = null!;
}
	
public enum TopUpRequestStatusEnum{
    PENDING, 
    PAID, 
    FAILED, 
    EXPIRED
}
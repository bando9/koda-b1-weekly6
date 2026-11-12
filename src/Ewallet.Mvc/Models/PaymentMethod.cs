namespace Ewallet.Mvc.Models;

public sealed class PaymentMethod
{
    public int Id {get; private set;}
    public string Code {get; private set;} = string.Empty;
    public string Name {get; private set;}= string.Empty;
    public string? ProviderName {get; private set;}
    public bool IsActive {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}

    // constraint topup reques
    public ICollection<TopUpRequest> TopUpRequests {get; private set;} = new List<TopUpRequest>();
}
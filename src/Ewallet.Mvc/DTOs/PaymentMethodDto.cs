using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.DTOs;

public sealed class PaymentMethodResponse
{
    public long Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? ProviderName { get; init; }
}

// public record PaymentMethodResponse(
//     long Id,
//     string Code, 
//     string Name, 
//     string? ProviderName, 
//     bool IsActive, 
//     DateTime CreatedAtUtc 
    
// )
// {
//     public static PaymentMethodResponse FromModel(PaymentMethod paymentMethod) => new(paymentMethod.Id, paymentMethod.Code,paymentMethod.Name,paymentMethod?.ProviderName,paymentMethod.IsActive,paymentMethod.CreatedAtUtc);
// };


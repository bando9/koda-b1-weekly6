using Ewallet.Mvc.Models;

namespace Ewallet.Mvc.DTOs;

public sealed record WalletResponse(long Id, string WalletNumber, long UserId, decimal Balance)
{
    public static WalletResponse FromModel(Wallet wallet) => new(wallet.Id,wallet.WalletNumber, wallet.UserId,wallet.Balance);
}
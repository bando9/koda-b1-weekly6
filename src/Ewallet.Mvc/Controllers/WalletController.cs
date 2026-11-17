using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Mvc.Controllers;

[ApiController]
[Route("api/v1/wallets")]

public sealed class WalletController (IHistoryService historyService) : ControllerBase
{
    [HttpGet("{walletId:long}/mutations")]
    [ProducesResponseType(typeof(PagedResult<MutationResponse>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMutations(
        long walletId, [FromQuery] MutationQuery query, CancellationToken ct)
        => Ok(await historyService.GetMutationsAsync(walletId, query, ct));
}
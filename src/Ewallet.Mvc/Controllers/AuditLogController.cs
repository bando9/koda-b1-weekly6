using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Models;
using Ewallet.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Mvc.Controllers;


[ApiController]
[Route("api/v1/audit-logs")]

public sealed class AuditLogController(IAuditLogService logService) : ControllerBase
{
    
    [HttpGet]
    [ProducesResponseType(typeof(List<AuditLog>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAll (CancellationToken ct)
    {
        var logs = await logService.GetAllAsync(ct);
        return Ok(logs);
    }
    
}
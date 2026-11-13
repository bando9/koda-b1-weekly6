using Ewallet.Mvc.DTOs;
using Ewallet.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Mvc.Controllers;

[ApiController]
[Route("api/v1/users")]

public sealed class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken ct)
    {
        var users = await _service.GetAllAsync(ct);
        return Ok(users);
    }

}
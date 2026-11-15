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
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetById(long id, CancellationToken ct)
    {
        var user = await _service.GetByIdAsync(id,ct);
        return user is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "User not found") : Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WalletResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WalletResponse>> Create([FromBody]UserRequest request ,CancellationToken ct)
    {
        try
        {
            var created = await _service.CreateWithWalletAsync(request, ct);

            if (created is null)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, title: "Wallet code already exists");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id }, created);
        }
        catch (InvalidOperationException e)
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Conflict user data!",
                detail: e.Message);
        }
        catch (ArgumentException e)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid user data!",
                detail: e.Message);
        }
    }


}
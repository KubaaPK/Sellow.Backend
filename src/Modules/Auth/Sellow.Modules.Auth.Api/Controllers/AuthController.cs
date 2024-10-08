using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sellow.Modules.Auth.Core.Features;
using Sellow.Shared.Infrastructure.Api;

namespace Sellow.Modules.Auth.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}")]
[ApiVersion("1.0")]
internal sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /users
    ///     {
    ///        "email": "jankowalski22@wp.pl",
    ///        "username": "jankowalski22",
    ///        "password": "super-secret-and-strong-password"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">User has been successfully created.</response>
    /// <response code="400">Request body validation has failed.</response>
    /// <response code="409">User with given credentials already exists.</response>
    /// <response code="500">Internal server error.</response>
    [ProducesResponseType(201)]
    [HttpPost("users")]
    public async Task<IResult> CreateUser([FromBody] CreateUser command, CancellationToken cancellationToken = default)
    {
        var userId = await _sender.Send(command, cancellationToken);
        return Results.Created($"{Request.GetActionUrl()}/users/{userId}", null);
    }

    /// <summary>
    /// Activates a user.
    /// </summary>
    /// <response code="200">User has been successfully activated.</response>
    /// <response code="422">User cannot be activated: was not found or is already active.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("auth/activate-user/{id:guid}")]
    public async Task<IResult> ActivateUser(Guid id, CancellationToken cancellationToken = default)
    {
        await _sender.Send(new ActivateUser(id), cancellationToken);
        return Results.Ok();
    }
}
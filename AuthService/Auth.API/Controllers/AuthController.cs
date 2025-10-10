using Auth.Application.Commands;
using Auth.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        var command = new RegisterCommand
        {
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("validate-token")]
    [Authorize]
    public async Task<ActionResult<ApiResponseDto<bool>>> ValidateToken()
    {
        return Ok( new ApiResponseDto<bool>
        {
            Success = true,
            Message = "Token is valid",
            Data = true
        });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        // Implementation for refresh token
        return Ok();
    }
}

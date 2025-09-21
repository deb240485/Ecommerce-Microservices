using Auth.Application.DTOs;
using MediatR;

namespace Auth.Application.Commands
{
    public class LoginCommand : IRequest<ApiResponseDto<AuthResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

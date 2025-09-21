using Auth.Application.DTOs;
using MediatR;

namespace Auth.Application.Commands
{
    public class RegisterCommand : IRequest<ApiResponseDto<AuthResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}

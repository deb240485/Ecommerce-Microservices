using Auth.Application.Commands;
using Auth.Application.DTOs;
using Auth.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponseDto<AuthResponseDto>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IAuthRepository authRepository, ITokenService tokenService, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<ApiResponseDto<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !await _authRepository.CheckPasswordAsync(user, request.Password))
            {
                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Errors = new List<string> { "Authentication failed" }
                };
            }

            if (!user.IsActive)
            {
                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "Account is deactivated",
                    Errors = new List<string> { "Account not active" }
                };
            }

            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Update user with refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!));
            await _authRepository.UpdateUserAsync(user);

            var roles = await _authRepository.GetUserRolesAsync(user);

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:AccessTokenValidityInMinutes"]!)),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Roles = roles.ToList()
                }
            };

            return new ApiResponseDto<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = response
            };
        }
    }
}

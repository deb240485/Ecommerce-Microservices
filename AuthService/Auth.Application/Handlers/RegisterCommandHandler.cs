using Auth.Application.Commands;
using Auth.Application.DTOs;
using Auth.Core.Entities;
using Auth.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponseDto<AuthResponseDto>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IAuthRepository authRepository,
            ITokenService tokenService,
            ILogger<RegisterCommandHandler> logger)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ApiResponseDto<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting user registration for email: {Email}", request.Email);

                // Validate the request
                var validationResult = ValidateRequest(request);
                if (!validationResult.IsValid)
                {
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = validationResult.Errors
                    };
                }

                // Check if user already exists
                var existingUser = await _authRepository.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration attempt failed - user already exists: {Email}", request.Email);
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "User with this email already exists",
                        Errors = new List<string> { "Email is already registered" }
                    };
                }

                // Create new user
                var normalizedEmail = request.Email.ToLowerInvariant().Trim();
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = normalizedEmail, // Using normalized email as username
                    Email = normalizedEmail,
                    FirstName = request.FirstName?.Trim(),
                    LastName = request.LastName?.Trim(),
                    EmailConfirmed = false, // You might want to implement email confirmation
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    RefreshTokenExpiryTime = DateTime.UtcNow // Will be updated when refresh token is generated
                };

                // Create user with password using Identity
                var createResult = await _authRepository.CreateUserAsync(user, request.Password);

                if (!createResult.Succeeded)
                {
                    _logger.LogError("Failed to create user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Failed to create user",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Add user to default role
                const string defaultRole = "User";
                if (await _authRepository.RoleExistsAsync(defaultRole))
                {
                    var addToRoleResult = await _authRepository.AddToRoleAsync(user, defaultRole);
                    if (!addToRoleResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to add user to default role: {Errors}",
                            string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                    }
                }

                _logger.LogInformation("User successfully registered with ID: {UserId}", user.Id);

                // Generate tokens
                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Update user with refresh token
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30); // 30 days expiry
                await _authRepository.UpdateUserAsync(user);

                // Get user roles for response
                var userRoles = await _authRepository.GetUserRolesAsync(user);

                // Create response
                var authResponse = new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpirationMinutes()),
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email!,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Roles = userRoles.ToList()
                    }
                };

                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = authResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration for email: {Email}", request.Email);
                return new ApiResponseDto<AuthResponseDto>
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { "Internal server error" }
                };
            }
        }

        private ValidationResult ValidateRequest(RegisterCommand request)
        {
            var errors = new List<string>();

            // Email validation
            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email is required");
            else if (!IsValidEmail(request.Email))
                errors.Add("Invalid email format");

            // Password validation
            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("Password is required");
            else if (request.Password.Length < 6)
                errors.Add("Password must be at least 6 characters long");

            // Confirm password validation
            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                errors.Add("Confirm password is required");
            else if (request.Password != request.ConfirmPassword)
                errors.Add("Password and confirm password do not match");

            // Name validation
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("First name is required");
            else if (request.FirstName.Length > 50)
                errors.Add("First name cannot exceed 50 characters");

            if (string.IsNullOrWhiteSpace(request.LastName))
                errors.Add("Last name is required");
            else if (request.LastName.Length > 50)
                errors.Add("Last name cannot exceed 50 characters");

            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        private int GetTokenExpirationMinutes()
        {
            // You might want to inject configuration for this
            return 60; // 60 minutes default
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
            public List<string> Errors { get; set; } = new();
        }
    }
}
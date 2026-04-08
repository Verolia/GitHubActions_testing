namespace Api.Features.AccountManagementContext.Shared.DTOs;

public record RegisterUserRequest(
    string Name,
    string EmailAddress,
    string Password
);
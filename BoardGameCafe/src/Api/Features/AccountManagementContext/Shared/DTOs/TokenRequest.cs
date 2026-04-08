namespace Api.Features.AccountManagementContext.Shared.DTOs;

public record TokenRequest(Guid UserId, string EmailAddress, string Role);
namespace Api.Features.AccountManagementContext.Shared.DTOs
{
    public class ChangePasswordRequest
    {
        public string EmailAddress { get; set; } = string.Empty; 
        public string CurrentPassword { get; set; } = string.Empty; 
        public string NewPassword { get; set; } = string.Empty; 
    }
}
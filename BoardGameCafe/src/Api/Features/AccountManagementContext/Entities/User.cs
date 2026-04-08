using Api.Features.AccountManagementContext.Enums;

namespace Api.Features.AccountManagementContext.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string Phone { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public bool ForcePasswordChange { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; set; }

    public User() { }

    public User(string name, string emailAddress, string phone, string passwordHash, UserRole role)
    {
        Id = Guid.NewGuid();
        Name = name;
        EmailAddress = emailAddress;
        Phone = phone;
        PasswordHash = passwordHash;
        Role = role;
    }
}
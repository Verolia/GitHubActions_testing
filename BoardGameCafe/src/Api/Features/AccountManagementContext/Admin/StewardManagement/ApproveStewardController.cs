using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Features.AccountManagementContext.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Api.Features.AccountManagementContext.Admin.ApproveSteward;

[ApiController]
[Route("auth/admin")]
[Authorize(Roles = "Admin")]
public class ApproveStewardController : ControllerBase
{
    private readonly AccountManagementDbContext _db;

    public ApproveStewardController(AccountManagementDbContext db)
    {
        _db = db;
    }

    [HttpPost("approve/{userId}")]
    public async Task<IActionResult> Approve(Guid userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound();

        user.IsActive = true;

        await _db.SaveChangesAsync();

        return Ok("User approved");
    }
}
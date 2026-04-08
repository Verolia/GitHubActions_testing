using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Api.Features.Test

// Endpoint example given by AI
{
    [ApiController]
    [Route("testauth")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("Public endpoint works");
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok("You are authenticated");
        }


        [Authorize(Roles = "Steward")]
        [HttpGet("steward")]
        public IActionResult Steward()
        {
            return Ok("You are Steward");
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult Admin()
        {
            return Ok("You are Admin");
        }
    }
}
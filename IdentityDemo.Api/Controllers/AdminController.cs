using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace IdentityDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet("panel")]
    [Authorize(Roles = "Admin")]
    public IActionResult Panel()
    {
        return Ok($"Wellcome to panel, {User.Identity?.Name}");
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Technical_Challenge.Entity.contacts;
using Technical_Challenge.Pagnations;
using Technical_Challenge.Services.IServices;

namespace Technical_Challenge.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserServices userServices) : ControllerBase
{
    private readonly IUserServices userServices = userServices;
    [HttpPost("Add-contact")]
    public async Task<IActionResult> AddContact([FromBody] Usercontact userContact, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID

        var result = await userServices.AddContact(userId! ,userContact , cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpGet("All-contact")]
    public async Task<IActionResult> GetAllContact([FromQuery] RequestFilters requestFilters, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.GetAllContact(userId! , requestFilters , cancellationToken);
        return Ok(result);

    }
    [HttpGet("Get-contact/{id}")]
    public async Task<IActionResult> GetContactById([FromRoute] int id, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.GetById(userId! , id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpDelete("delete-contact/{id}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.DeleteContact(userId!, id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("Update-contact")]
    public async Task<IActionResult> UpdateContact( [FromBody] Usercontact userContact, CancellationToken cancellationToken)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Extracts ID
        var result = await userServices.UpdateContact(userId! , userContact, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Technical_Challenge.Abstractions;
using Technical_Challenge.Services.IServices;

namespace Technical_Challenge.Controllers;
[Route("api/[controller]")]
[ApiController]
public class authenticatController(IAuthenticatServices authenticatServices) : ControllerBase
{
    private readonly IAuthenticatServices authenticatServices = authenticatServices;
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        var result = await authenticatServices.RegisterAsync(userRegister);
       
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] userLogin userLogin, CancellationToken cancellationToken)
    {
        var result = await authenticatServices.LoginAsync(userLogin , cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] UserRefreshToken userRefreshToken, CancellationToken cancellationToken)
    {
        var result = await authenticatServices.GetRefreshToken(userRefreshToken , cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

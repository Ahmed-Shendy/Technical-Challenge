using Technical_Challenge.Abstractions;

namespace Technical_Challenge.Services.IServices;

public interface IAuthenticatServices
{
    Task<Result<UserRespones>> RegisterAsync(UserRegister userRequest, CancellationToken cancellationToken = default);
    Task<Result<UserRespones>> LoginAsync(userLogin userLogin, CancellationToken cancellationToken = default);
    Task<Result<UserRespones>> GetRefreshToken(UserRefreshToken request, CancellationToken cancellationToken = default);
}

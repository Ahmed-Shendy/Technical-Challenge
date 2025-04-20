using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using Technical_Challenge.Abstractions;
using Technical_Challenge.Errors;
using Technical_Challenge.model;
using Technical_Challenge.model.Context;
using Technical_Challenge.Outherize;
using Technical_Challenge.Services.IServices;

namespace Technical_Challenge.Services;

public class AuthenticatServices(ApplicationDbContext applicationDbContext
    , token token,
     UserManager<User> user
    , SignInManager<User> signInManager
    , ILogger<AuthenticatServices> logger
    ) : IAuthenticatServices
{
    private readonly ApplicationDbContext db = applicationDbContext;
    private readonly token Token = token;
    private readonly UserManager<User> _userManager = user;
    private readonly SignInManager<User> signInManager = signInManager;
    private readonly ILogger<AuthenticatServices> logger = logger;
    private readonly int RefreshTokenDays = 14;


    public async Task<Result<UserRespones>> RegisterAsync(UserRegister userRequest, CancellationToken cancellationToken = default)
    {


        var emailIsExists = await db.Users.AnyAsync(x => x.Email == userRequest.Email, cancellationToken);
        if (emailIsExists)
            return Result.Failure<UserRespones>(UserErrors.EmailUnque);

        var request = userRequest.Adapt<User>();
        request.UserName = request.Email;

        var save = await _userManager.CreateAsync(request, request.Password);
        await db.SaveChangesAsync(cancellationToken);
        if (save.Succeeded)
        {

            await db.SaveChangesAsync(cancellationToken);
            var user = await db.Users.SingleOrDefaultAsync(x => x.Email == userRequest.Email);
            var result = user.Adapt<UserRespones>();
            
            
            var (token, expiresIn) = Token.GenerateToken(result);
            result.Token = token;
            result.ExpiresIn = expiresIn;
            result.RefreshToken = GenerateRefreshToken();
            result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);
            user!.RefreshTokens.Add(new RefreshToken
            {
                Token = result.RefreshToken,
                ExpiresOn = result.RefreshTokenExpiretion,
            });
            await db.SaveChangesAsync(cancellationToken);

            return Result.Success(result);
        }

        var error = save.Errors.ToList();
        logger.LogError("User creation failed: {error}", error[0].Description);

        return Result.Failure<UserRespones>(UserErrors.notsaved);

    }

    public async Task<Result<UserRespones>> LoginAsync(userLogin userLogin, CancellationToken cancellationToken = default)
    {

        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == userLogin.Email, cancellationToken);

        if (user is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);

        var isValidPassword = await signInManager.PasswordSignInAsync(user, userLogin.Password, false, true);


        if (user.LockoutEnd != null)
        {
            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<UserRespones>(UserErrors.LookUser);
        }
        if (!isValidPassword.Succeeded)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);

        var result = user.Adapt<UserRespones>();


        var (token, expiresIn) = Token.GenerateToken(result);
        result.Token = token;
        result.ExpiresIn = expiresIn;
        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion,
        });
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success(result);
    }



    public async Task<Result<UserRespones>> GetRefreshToken(UserRefreshToken request, CancellationToken cancellationToken = default)
    {

        var UserEmail = Token.ValisationToken(request.Token);

        if (UserEmail is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);


        var user = await db.Users.SingleOrDefaultAsync(i => i.Email == UserEmail, cancellationToken);

        if (user is null)
            return Result.Failure<UserRespones>(UserErrors.UserNotFound);


        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefrehToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<UserRespones>(UserErrors.InvalidToken);

        // ***to stop Refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var result = user.Adapt<UserRespones>();
      

        var (newtoken, expiresIn) = Token.GenerateToken(result);
        result.Token = newtoken;
        result.ExpiresIn = expiresIn;

        result.RefreshToken = GenerateRefreshToken();
        result.RefreshTokenExpiretion = DateTime.UtcNow.AddDays(RefreshTokenDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = result.RefreshToken,
            ExpiresOn = result.RefreshTokenExpiretion
        });
        await db.SaveChangesAsync(cancellationToken);
        await _userManager.UpdateAsync(user);

        return Result.Success(result);
    }


    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(count: 64));
    }


}

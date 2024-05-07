using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Diploma.Common.ServicesForWeb;

public class CookieAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly CookieService cookieService;
    private readonly SessionsService _sessionsService;

    public CookieAuthenticationStateProvider(CookieService cookieService, SessionsService sessionsService)
    {
        this.cookieService = cookieService;
        _sessionsService = sessionsService;
    }
    
    // Метод для проверки срока действия куки
    public async Task<bool> IsCookieExpired()
    {
        var cookieExpiration = await GetCookieExpiration("token");
        if (cookieExpiration == null)
        {
            // Куки не имеют времени истечения, считаем их действительными
            return false;
        }
        else if ((DateTime.UtcNow - cookieExpiration.Value) > TimeSpan.FromHours(1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // if (await IsCookieExpired())
        // {
        //     return new AuthenticationState(new ClaimsPrincipal());
        // }

        var token = await cookieService.GetCookies("token");
        
        if (!string.IsNullOrEmpty(token))
        {
            await _sessionsService.RefreshSession();
            var claims = new[] { new Claim(ClaimTypes.Authentication, token) }; // Замените "username" на фактическую информацию о пользователе
            var identity = new ClaimsIdentity(claims, "token");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        return GetStateAnonymous();
    }
    
    public async Task SetAuthenticationState(string userName)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "token");
        var user = new ClaimsPrincipal(identity);
    
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }
    
    private async Task<DateTime?> GetCookieExpiration(string key)
    {
        var expiration = await cookieService.GetCookies(key + "_Expires");
        if (!string.IsNullOrEmpty(expiration) && DateTime.TryParse(expiration, out DateTime expirationDateTime))
        {
            return expirationDateTime;
        }

        // Возвращаем null, если срок действия куки не найден
        return null;
    }
    
    private static AuthenticationState GetStateAnonymous()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        return new AuthenticationState(anonymous);
    }
}
using System.Security.Claims;
using Diploma.API.Services;
using Diploma.Common.Interfaces;
using Diploma.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace Diploma.Web.Utils;

public class CookieAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly CookiesService cookiesService;
    private readonly SessionService SessionService;

    public CookieAuthenticationStateProvider(CookiesService cookiesService, SessionService sessionService)
    {
        this.cookiesService = cookiesService;
        SessionService = sessionService;
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

        var session = SessionService.ReadSession();
        
        if (session != null && !string.IsNullOrEmpty(session.Token))
        {
            var claims = new[] { new Claim(ClaimTypes.Name, session.User.Name) }; // Замените "username" на фактическую информацию о пользователе
            var identity = new ClaimsIdentity(claims, "token");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        return new AuthenticationState(new ClaimsPrincipal());
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
        var expiration = await cookiesService.GetCookie(key + "_Expires");
        if (!string.IsNullOrEmpty(expiration) && DateTime.TryParse(expiration, out DateTime expirationDateTime))
        {
            return expirationDateTime;
        }

        // Возвращаем null, если срок действия куки не найден
        return null;
    }
}
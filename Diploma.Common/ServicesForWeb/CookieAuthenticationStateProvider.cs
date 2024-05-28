using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Diploma.Common.Models;
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
        var token = await cookieService.GetCookies("token");
        
        if (!string.IsNullOrEmpty(token))
        {
            await _sessionsService.RefreshSession();
            var role = _sessionsService.SessionData.User.Role;
            var claims = new[]
            {
                new Claim(ClaimTypes.Authentication, token),
                new Claim(ClaimTypes.Role, role.ToString())
            };
            var identity = new ClaimsIdentity(claims, "token");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        return GetStateAnonymous();
    }
    
    private async Task<DateTime?> GetCookieExpiration(string key)
    {
        var expiration = await cookieService.GetCookies(key + "_Expires");
        if (!string.IsNullOrEmpty(expiration) && DateTime.TryParse(expiration as string, out DateTime expirationDateTime))
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
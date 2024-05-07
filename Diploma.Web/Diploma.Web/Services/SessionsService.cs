using Diploma.API.Services;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Services;

namespace Diploma.Web.Services;

public class SessionsService
{
    public Func<Session, Task> OnRefreshSession { get; set; }
    public Session SessionData { get; set; }
    
    private readonly CookiesService _cookiesService;
    private readonly CookieService CookieService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly SessionCacheService _sessionCacheService;
    private readonly IAccountService AccountService;

    public SessionsService(IAccountService accountService, CookiesService cookiesService, SessionCacheService sessionCacheService,
        IHttpContextAccessor httpContextAccessor, CookieService cookieService)
    {
        _cookiesService = cookiesService;
        this.httpContextAccessor = httpContextAccessor;
        _sessionCacheService = sessionCacheService;
        AccountService = accountService;
        CookieService = cookieService;
    }
    public async Task RefreshSession()
    {
        SessionData = await AccountService.Authenticate();
        if (OnRefreshSession != null) await OnRefreshSession(SessionData);
    }
    public async Task<Session> ReadSession(string token)
    {
        // string token = await CookieService.GetCookies("token");

        if (string.IsNullOrWhiteSpace(token)) return default;
        var session = _sessionCacheService.GetSessionFromCache(token);
        return session;
    }
    public async Task<Session> Create(User user)
    {
        Session session = new Session
        {
            User = user,
            Token = user.Token,
            DateTime = DateTime.UtcNow
        };
        _sessionCacheService.AddSessionToCache(session.Token, session);
        return session;
    }
    private async Task<string> GetCookie(string key)
    {
        var context = httpContextAccessor.HttpContext;
        var token = await Task.FromResult(context.Request.Cookies[key]);
        return token; // Дождаться выполнения задачи
    }
}
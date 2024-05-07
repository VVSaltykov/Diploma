using Diploma.Common.Models;
using Diploma.Common.Services;
using Microsoft.AspNetCore.Http;

namespace Diploma.API.Services;

public class SessionService
{
    protected Dictionary<string, Session> sessions = new();
    
    private readonly IHttpContextAccessor httpContextAccessor;
    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<Session> Create(User user)
    {
        Session session = new Session
        {
            User = user,
            Token = user.Token,
            DateTime = DateTime.UtcNow
        };
        lock (sessions)
        {
            sessions.Add(session.Token, session);
        }
        return session;
    }

    public Session ReadSession(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return default;

        lock (sessions)
        {
            return sessions.GetValueOrDefault(key);
        }
    }

    public Session ReadSession(HttpContext context = null)
    {
        context ??= httpContextAccessor.HttpContext;
        var key = GetKey(context);
        return ReadSession(key);
    }

    public async Task<Session?> Get(Dictionary<string, Session> sessions, string token)
    {
        Session? session = sessions.Values.FirstOrDefault(s => s.Token == token);
        return session;
    }
    
    private string GetKey(HttpContext context)
    {
        return context.Request.Headers["AuthorizationToken"];
    }

    private void DeleteExpiredSessions()
    {
        lock (sessions)
        {
            var sessionsKeysToDelete = sessions.Where(s => s.Value.DateTime <= DateTime.UtcNow).Select(s => s.Key).ToList();
            sessionsKeysToDelete.ForEach(str => sessions.Remove(str));
        }
    }
}
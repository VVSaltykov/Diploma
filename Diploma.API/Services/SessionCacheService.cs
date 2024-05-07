using Diploma.Common.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Diploma.API.Services;

public class SessionCacheService
{
    private readonly IMemoryCache _cache;

    public SessionCacheService(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    public void AddSessionToCache(string token, Session session)
    {
        _cache.Set(token, session, TimeSpan.FromMinutes(30)); // Пример: срок действия 30 минут
    }

    public Session? GetSessionFromCache(string token)
    {
        return _cache.Get<Session>(token);
    }
}
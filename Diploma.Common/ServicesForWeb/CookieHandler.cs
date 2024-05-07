using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Diploma.Common.ServicesForWeb;

public class CookieHandler : DelegatingHandler
{
    private readonly CookieService CookieService;

    public CookieHandler(CookieService CookieService)
    {
        this.CookieService = CookieService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Получаем токен из HttpContext
        var token = await CookieService.GetCookies("token");
        
        // Добавляем токен в заголовок Authorization, если он есть
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
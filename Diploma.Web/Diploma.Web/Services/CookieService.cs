using System.Security.Claims;
using System.Text;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Diploma.Web.Services;

public class CookieService
{
    private readonly IJSRuntime jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task SetCookies(string key, string value)
    {
        var expirationTime = DateTime.UtcNow;
        await jsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", key, value, expirationTime);
        await jsRuntime.InvokeAsync<object>("WriteCookie.WriteCookie", key + "_Expires", expirationTime.ToString(), expirationTime);
    }

    public async Task<string> GetCookies(string key)
    {
        return await jsRuntime.InvokeAsync<string>("ReadCookie.ReadCookie", key);
    }
}
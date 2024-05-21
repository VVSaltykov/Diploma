using Diploma.Common.Interfaces;
using Diploma.Common.Services;
using Diploma.Common.ServicesForWeb;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Diploma.WebClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        
        
        builder.Services.AddTransient<CookieHandler>();
        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7165") };
            // Configure HttpClient here if needed
            return httpClient;
        });
        builder.Services.AddSingleton<SessionsService>();
        
        builder.Services.AddAuthorizationCore();
        
        builder.Services.AddScoped<Radzen.DialogService>();
        builder.Services.AddScoped<Radzen.NotificationService>();
        builder.Services.AddScoped<Radzen.ContextMenuService>();
        builder.Services.AddScoped<Radzen.TooltipService>();

// Настраиваем HttpClient после его создания, добавляя к нему CookieHandler
        builder.Services.AddHttpClient<IAccountService, AccountService>("API", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7165");
        }).AddHttpMessageHandler<CookieHandler>();
        
        
        builder.Services.AddSingleton<IMessagesService, MessagesService>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            httpClient.BaseAddress = new Uri("https://localhost:7165");
            // Можно настроить HttpClient здесь, если необходимо
            return new MessagesService(httpClient);
        });
        builder.Services.AddSingleton<MessagesService>();
        
        builder.Services.AddSingleton<IUserService, UserService>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            httpClient.BaseAddress = new Uri("https://localhost:7165");
            // Можно настроить HttpClient здесь, если необходимо
            return new UserService(httpClient);
        });
        builder.Services.AddSingleton<UserService>();
        
        builder.Services.AddSingleton<IGroupService, GroupService>(sp =>
        {
            var httpClient = sp.GetRequiredService<HttpClient>();
            httpClient.BaseAddress = new Uri("https://localhost:7165");
            // Можно настроить HttpClient здесь, если необходимо
            return new GroupService(httpClient);
        });
        builder.Services.AddSingleton<GroupService>();
        builder.Services.AddScoped<CookieService>();
        builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CookieAuthenticationStateProvider>());
        builder.Services.AddScoped<CookieAuthenticationStateProvider>();
        await builder.Build().RunAsync();
    }
}
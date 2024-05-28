
using System;
using System.Threading.Tasks;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Services;

namespace Diploma.Common.ServicesForWeb;

public class SessionsService
{
    public Func<Session, Task> OnRefreshSession { get; set; }
    public Session SessionData { get; set; }
    
    private readonly IAccountService AccountService;

    public SessionsService(IAccountService accountService)
    {
        AccountService = accountService;
    }
    public async Task RefreshSession()
    {
        SessionData = await AccountService.Authenticate();
        if (OnRefreshSession != null) await OnRefreshSession(SessionData);
    }
}
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IAccountService
{
    [Post("/api/Account")]
    Task Registration([FromBody] RegistrationModel model);
}
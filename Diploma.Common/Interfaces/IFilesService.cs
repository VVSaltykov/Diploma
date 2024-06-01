using Diploma.Common.Models;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IFilesService
{
    [Post("/api/Files/Create")]
    Task Create(Files file);

    [Post("/api/Files/GetFiles")]
    Task<List<Files>> GetFiles(List<string> filesIds);

    [Post("/api/Files/Update")]
    Task Update(Files file);
}
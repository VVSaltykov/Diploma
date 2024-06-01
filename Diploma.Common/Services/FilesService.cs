using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;

namespace Diploma.Common.Services;

public class FilesService : IFilesService
{
    private readonly IFilesService _filesService;

    public FilesService(HttpClient httpClient)
    {
        _filesService = RefitFunctions.GetRefitService<IFilesService>(httpClient);
    }
    
    public async Task Create(Files file)
    {
        await _filesService.Create(file);
    }

    public async Task<List<Files>> GetFiles(List<string> filesIds)
    {
        var files = await _filesService.GetFiles(filesIds);
        return files;
    }

    public async Task Update(Files file)
    {
        await _filesService.Update(file);
    }
}
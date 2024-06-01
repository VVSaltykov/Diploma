using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FilesRepository FilesRepository;

    public FilesController(FilesRepository filesRepository)
    {
        FilesRepository = filesRepository;
    }
    
    [HttpPost("Create")]
    public async Task Create(Files file)
    {
        await FilesRepository.Create(file);
    }
    
    [HttpPost("GetFiles")]
    public async Task<List<Files>> GetFiles(List<string> filesIds)
    {
        var files = new List<Files>();

        foreach (var filesId in filesIds)
        {
            files.Add(await FilesRepository.ReadFirst(f => f.FileId == filesId));
        }

        return files;
    }
    
    [HttpPost("Update")]
    public async Task Update(Files file)
    {
        await FilesRepository.Update(file);
    }
}
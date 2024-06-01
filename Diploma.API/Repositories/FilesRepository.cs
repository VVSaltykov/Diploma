using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class FilesRepository : BaseCRUDRepository<Files, int>
{
    private readonly AppDbContext _appDbContext;
    
    public FilesRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }
}
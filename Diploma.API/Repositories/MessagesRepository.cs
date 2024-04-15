using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class MessagesRepository : BaseCRUDRepository<Messages, int>
{
    private readonly AppDbContext _appDbContext;
    
    public MessagesRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }
}
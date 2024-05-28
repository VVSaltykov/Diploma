using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class AchievementsRepository : BaseCRUDRepository<Achievements, int>
{
    private readonly AppDbContext _appDbContext;
    
    public AchievementsRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }
}
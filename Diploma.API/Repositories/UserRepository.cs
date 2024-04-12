using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class UserRepository
{
    private readonly AppDbContext AppDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        AppDbContext = appDbContext;
    }

    public async Task Create(long chatId, string phoneNumber)
    {
        User user = new User
        {
            ChatId = chatId,
            PhoneNumber = phoneNumber
        };
        AppDbContext.Users.Add(user);
        await AppDbContext.SaveChangesAsync();
    }
}
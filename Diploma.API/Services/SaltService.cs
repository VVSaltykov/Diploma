using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Diploma.API.Services;

public class SaltService
{
    private readonly SaltRepository _saltRepository;
    private readonly UserRepository UserRepository;

    public SaltService(SaltRepository saltRepository, UserRepository userRepository)
    {
        _saltRepository = saltRepository;
        UserRepository = userRepository;
    }
    
    public async Task<string> HashData(string data, User user = null)
    {
        Random random = new Random();
        if (user.Salt.HashSalt != null)
        {
            data = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: data,
                salt: user.Salt.HashSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return data;
        }
        else
        {
            Salt salt = new Salt();
        
            salt.HashSalt = new byte[10];
            random.NextBytes(salt.HashSalt);

            data = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: data,
                salt: salt.HashSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            await _saltRepository.Create(salt);
            user.Salt = salt;
            return data;
        }
    }

    public async Task<string> GetHashData(string data, User user = null, string login = null)
    {
        if (user == null)
        {
            var _user = (await UserRepository.Read(u => u.Login == login, include: u => u.Salt)).FirstOrDefault();
            data = await HashData(data, _user);
            return data;
        }
        else
        {
            data = await HashData(data, user);
            return data;
        }
    }
}
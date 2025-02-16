using CryptoWallet.Domain.Models;

namespace CryptoWallet.Domain.Interface;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User> CreateUserAsync(User user);
    Task UpdateBalanceAsync(Guid userId, decimal newBalance);
}
using CryptoWallet.Domain.Interface;
using CryptoWallet.Domain.Models;
using CryptoWallet.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly CryptoWalletContext _context;

    public UserRepository(CryptoWalletContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateBalanceAsync(Guid userId, decimal newBalance)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("Пользователь не найден");

            user.Balance = newBalance;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
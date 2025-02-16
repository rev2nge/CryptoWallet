using CryptoWallet.Application.Dto;
using CryptoWallet.Application.Service.Interface;
using CryptoWallet.Domain.Interface;
using CryptoWallet.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CryptoWallet.Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<IUserRepository> _logger;

    public UserService(IUserRepository userRepository, ILogger<IUserRepository> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<RegisterUserDto?> RegisterUserAsync(RegisterEmailDto registerUser)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(registerUser.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Попытка регистрации пользователя с существующим в базе email.");
            throw new KeyNotFoundException($"Пользователь с email {registerUser.Email} уже зарегистрирован.");
        }
        
        var user = new User { Email = registerUser.Email };
        var createdUser = await _userRepository.CreateUserAsync(user);
        return new RegisterUserDto { Id = createdUser.UserId, Email = createdUser.Email, Balance = createdUser.Balance };
    }

    public async Task<UserDto?> GetBalanceAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user != null ? new UserDto { Id = user.UserId, Balance = user.Balance } : null;
    }

    public async Task<BalanceDto?> DepositAsync(Guid userId, ChangeBalanceDto changeBalanceDto)
    {
        if (changeBalanceDto.Amount <= 0)
        {
            _logger.LogWarning("Попытка пополнения на некорректную сумму: {Amount}", changeBalanceDto.Amount);
            return null;
        }

        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Пользователь с ID {UserId} не найден", userId);
                throw new KeyNotFoundException($"Пользователь с ID {userId} не найден.");
            }

            await _userRepository.UpdateBalanceAsync(userId, user.Balance + changeBalanceDto.Amount);
            _logger.LogInformation("Пополнение {Id} на сумму {Amount}. Новый баланс: {Balance}", user.UserId, changeBalanceDto.Amount, user.Balance);
            return new BalanceDto { Id = user.UserId, NewBalance = user.Balance };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при пополнении счета {Id}", userId);
            return null;
        }
    }

    public async Task<BalanceDto?> WithdrawAsync(Guid userId, ChangeBalanceDto changeBalanceDto)
    {
        if (changeBalanceDto.Amount <= 0)
        {
            _logger.LogWarning("Попытка снятия некорректной суммы: {Amount}", changeBalanceDto.Amount);
            return null;
        }

        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Пользователь с ID {UserId} не найден", userId);
                throw new KeyNotFoundException($"Пользователь с ID {userId} не найден.");
            }
            
            if (user == null || user.Balance < changeBalanceDto.Amount)
            {
                _logger.LogWarning("Недостаточно средств для пользователя {Id}", userId);
                return null;
            }

            await _userRepository.UpdateBalanceAsync(userId, user.Balance - changeBalanceDto.Amount);
            _logger.LogInformation("Снятие {Amount} у пользователя {Id}. Новый баланс: {Balance}", changeBalanceDto.Amount, user.UserId, user.Balance);
            return new BalanceDto { Id = user.UserId, NewBalance = user.Balance };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при снятии средств у {Id}", userId);
            return null;
        }
    }
}
using CryptoWallet.Application.Dto;
using CryptoWallet.Domain.Models;

namespace CryptoWallet.Application.Service.Interface;

public interface IUserService
{
    Task<RegisterUserDto?> RegisterUserAsync(RegisterEmailDto email);
    Task<UserDto?> GetBalanceAsync(Guid userId);
    Task<BalanceDto?> DepositAsync(Guid userId, ChangeBalanceDto changeBalanceDto);
    Task<BalanceDto?> WithdrawAsync(Guid userId, ChangeBalanceDto changeBalanceDto);
}
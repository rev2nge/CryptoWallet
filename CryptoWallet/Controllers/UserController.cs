using CryptoWallet.Application.Dto;
using CryptoWallet.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterEmailDto registerUser)
    {
        var user = await _userService.RegisterUserAsync(registerUser);
        return Ok(user);
    }

    [HttpGet("{userId}/balance")]
    public async Task<IActionResult> GetBalance(Guid userId)
    {
        var balance = await _userService.GetBalanceAsync(userId);
        return balance != null ? Ok(balance) : NotFound();
    }

    [HttpPost("{userId}/deposit")]
    public async Task<IActionResult> Deposit(Guid userId, ChangeBalanceDto changeBalance)
    {
        var updatedUser = await _userService.DepositAsync(userId, changeBalance);
        return updatedUser != null ? Ok(updatedUser) : BadRequest();
    }

    [HttpPost("{userId}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid userId, ChangeBalanceDto changeBalance)
    {
        var updatedUser = await _userService.WithdrawAsync(userId, changeBalance);
        return updatedUser != null ? Ok(updatedUser) : BadRequest(new { error = "Insufficient funds" });
    }
}
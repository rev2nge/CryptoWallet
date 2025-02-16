using CryptoWallet.Application.Dto;
using CryptoWallet.Application.Service.Interface;
using CryptoWallet.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CryptoWallet.Test;

public class UserTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;
    private readonly ITestOutputHelper _output;

    public UserTests(ITestOutputHelper output)
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
        _output = output; 
    }

    [Fact]
    public async Task RegisterUserTest()
    {
        var registerEmail = new RegisterEmailDto { Email = "test@example.com" };
        var expectedUser = new RegisterUserDto { Id = Guid.NewGuid(), Email = registerEmail.Email, Balance = 0m };

        _mockUserService.Setup(service => service.RegisterUserAsync(registerEmail)).ReturnsAsync(expectedUser);

        _output.WriteLine($"Test Register: email = {registerEmail.Email}, expectedUser = {expectedUser.Email}");

        var result = await _controller.Register(registerEmail);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<RegisterUserDto>(okResult.Value);
        Assert.Equal(expectedUser.Email, returnValue.Email);

        _output.WriteLine($"Test Register passed: {returnValue.Email}");
    }

    [Fact]
    public async Task GetBalanceTest()
    {
        var userId = Guid.NewGuid();
        var expectedBalance = new UserDto { Id = userId, Balance = 100m };

        _mockUserService.Setup(service => service.GetBalanceAsync(userId)).ReturnsAsync(expectedBalance);

        _output.WriteLine($"Test GetBalance: userId = {userId}, expectedBalance = {expectedBalance.Balance}");

        var result = await _controller.GetBalance(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(expectedBalance.Balance, returnValue.Balance);

        _output.WriteLine($"Test GetBalance passed: {returnValue.Balance}");
    }
    
    [Fact]
    public async Task DepositTest()
    {
        var userId = Guid.NewGuid();
        var amount = 50m;
        var changeBalance = new ChangeBalanceDto { Amount = amount };
        var balance = new BalanceDto { Id = userId, NewBalance = 150m };
    
        _mockUserService.Setup(service => service.DepositAsync(userId, changeBalance))
            .ReturnsAsync(balance); 
    
        _output.WriteLine($"Test Deposit: userId = {userId}, amount = {amount}, updatedBalance = {balance.NewBalance}");
    
        var result = await _controller.Deposit(userId, changeBalance);
    
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<BalanceDto>(okResult.Value);
        Assert.Equal(balance.NewBalance, returnValue.NewBalance);
    
        _output.WriteLine($"Test Deposit passed: {returnValue.NewBalance}");
    }

    [Fact]
    public async Task InsufficientFundsTest()
    {
        var userId = Guid.NewGuid();
        var amount = 50m;
        var changeBalance = new ChangeBalanceDto { Amount = amount };
        
        _mockUserService.Setup(service => service.WithdrawAsync(userId, changeBalance))
            .ReturnsAsync((BalanceDto?)null);

        _output.WriteLine($"Test Withdraw (Insufficient Funds): userId = {userId}, amount = {amount}");

        var result = await _controller.Withdraw(userId, changeBalance);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var error = badRequestResult.Value;
        Assert.Equal("Insufficient funds", error);

        _output.WriteLine("Test Withdraw (Insufficient Funds) passed");
    }

    [Fact]
    public async Task WithdrawTest()
    {
        var userId = Guid.NewGuid();
        var amount = 50m;
        var changeBalance = new ChangeBalanceDto { Amount = amount };
        var balance = new BalanceDto { Id = userId, NewBalance = 150m };

        _mockUserService.Setup(service => service.WithdrawAsync(userId, changeBalance))
            .ReturnsAsync(balance);

        _output.WriteLine($"Test Withdraw: userId = {userId}, amount = {amount}, updatedBalance = {balance.NewBalance}");

        var result = await _controller.Withdraw(userId, changeBalance);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<BalanceDto>(okResult.Value);
        Assert.Equal(balance.NewBalance, returnValue.NewBalance);

        _output.WriteLine($"Test Withdraw passed: {returnValue.NewBalance}");
    }
}
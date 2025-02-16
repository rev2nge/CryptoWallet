namespace CryptoWallet.Domain.Models;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
}
using CryptoWallet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Infrastructure.Context;

public class CryptoWalletContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public CryptoWalletContext(DbContextOptions<CryptoWalletContext> options) : base(options) { }
}

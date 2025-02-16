using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CryptoWallet.Infrastructure.Context;

public class CryptoWalletContextFactory : IDesignTimeDbContextFactory<CryptoWalletContext>
{
    public CryptoWalletContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        var optionsBuilder = new DbContextOptionsBuilder<CryptoWalletContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(CryptoWalletContext).Assembly.FullName));

        return new CryptoWalletContext(optionsBuilder.Options);
    }
}
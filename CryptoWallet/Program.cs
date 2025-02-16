using CryptoWallet.Application.Service;
using CryptoWallet.Application.Service.Interface;
using CryptoWallet.Domain.Interface;
using CryptoWallet.Infrastructure.Context;
using CryptoWallet.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<CryptoWalletContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Statistic.Infrastructure")));

services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

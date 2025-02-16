using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Application.Dto;

public class ChangeBalanceDto
{
    [Required]
    [Range(0, 10000)]
    public decimal Amount { get; set; }
}
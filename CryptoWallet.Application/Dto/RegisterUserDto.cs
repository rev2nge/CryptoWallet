﻿using System.ComponentModel.DataAnnotations;

namespace CryptoWallet.Application.Dto;

public class RegisterUserDto : UserDto 
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
}

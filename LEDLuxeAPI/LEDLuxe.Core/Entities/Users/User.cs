﻿namespace LEDLuxe.Core.Entities.Users;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }
}
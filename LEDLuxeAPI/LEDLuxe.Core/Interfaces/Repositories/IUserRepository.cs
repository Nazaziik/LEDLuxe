﻿using LEDLuxe.Core.Entities.Users;

namespace LEDLuxe.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);

    Task<User> GetByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllAsync();

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(Guid userId);
}
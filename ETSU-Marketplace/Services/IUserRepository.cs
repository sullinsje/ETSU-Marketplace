using System;
using ETSU_Marketplace.Models;

/// <summary>
/// Provides methods for looking up users by username and user ID and
/// viewing user profile details.
/// </summary>

namespace ETSU_Marketplace.Services;

/// <summary>
/// Defines the methods the UserRepository will use  
/// </summary>
public interface IUserRepository
{
    Task<ApplicationUser?> ReadByUsernameAsync(string username);
    Task<ApplicationUser?> ReadByIdAsync(string userId);
    Task<ApplicationUser?> ReadProfileAsync(string userId);


}
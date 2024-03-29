﻿namespace Ecommerce.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email,string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        Task<bool> UserExists(string email);
        int GetUserId();
        string GetUserEmail();
        Task<User> GetUserByEmail(string email);
       
    }
}

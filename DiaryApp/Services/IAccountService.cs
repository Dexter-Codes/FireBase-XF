using System;
using System.Threading.Tasks;
using DiaryApp.Model;

namespace DiaryApp.Services
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(string username, string password);

        Task<AuthenticatedUser> GetUsersAsync();
    }
}

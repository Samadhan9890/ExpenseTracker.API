using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Contracts.IContracts
{
    public interface IUserFactory
    {
        public Task<ResponseDto> GetUsersAsync();
        public Task<ResponseDto> GetUserAsync(int userId);
        public Task<ResponseDto> CreateUser(UserDto userDto);
        public Task<ResponseDto> UpdateUser(UserDto userDto);
        public Task<ResponseDto> DeleteUser(int userId);
        public Task<ResponseDto> UpdatePassword(int id, ChangePasswordModel userPassword, string fields);
    }
}

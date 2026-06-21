using ExpenseTracker.Services.Models.DTOs;

namespace ExpenseTracker.Services.Services.IServices
{
    public interface IRoleMasterService
    {
        Task<ResponseDto> GetAllRoles();
        Task<ResponseDto> GetRoleById(int id);
        Task<ResponseDto> AddRole(RoleDto roleDto);
        Task<ResponseDto> UpdateRole(RoleDto roleDto);
        Task<ResponseDto> DeleteRole(int id);
    }
}

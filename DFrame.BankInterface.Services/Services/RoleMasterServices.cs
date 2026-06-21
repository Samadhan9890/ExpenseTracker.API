using AutoMapper;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace ExpenseTracker.Services.Services
{
    public class RoleMasterServices: IRoleMasterService
    {
        private readonly IMasterRepository _masterRepository;
        private ResponseDto _responseDto;
        private readonly ILogger<BankMasterServices> _logger;
        private readonly IMapper _mapper;
        public RoleMasterServices(IMasterRepository masterRepository, ResponseDto responseDto, ILogger<BankMasterServices> logger, IMapper mapper)
        {
            _masterRepository = masterRepository;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetAllRoles()
        {
            _logger.LogInformation($"Get role list");
            try
            {
                var roles = await _masterRepository.GetAllRoles();
                List<RoleDto> roleDto = _mapper.Map<List<RoleDto>>(roles);
                _responseDto.Result = roleDto;
                _logger.LogInformation($"Retunrining role list");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get role list";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetRoleById(int id)
        {
            _logger.LogInformation($"Get role for ID {id}");
            try
            {
                var role = await _masterRepository.GetRoleById(id);
                RoleDto roleDto = _mapper.Map<RoleDto>(role);
                _responseDto.Result = roleDto;
                _logger.LogInformation($"Retunrining role Details for ID {id}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to get role error - {ex.Message}";
            }

            return _responseDto;
        }

        public async Task<ResponseDto> AddRole(RoleDto roleDto)
        {
            try
            {
                _logger.LogInformation($"Role Creation Started.");
                TblRole tblRole = _mapper.Map<TblRole>(roleDto);
                tblRole = await _masterRepository.AddRole(tblRole);
                RoleDto createRole = _mapper.Map<RoleDto>(tblRole);
                _responseDto.Result = createRole;
                _logger.LogInformation($"Role Created Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to add role error - {ex.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateRole(RoleDto roleDto)
        {
            try
            {
                _logger.LogInformation($"Update Role with parameters ");
                TblRole tblRole = _mapper.Map<TblRole>(roleDto);
                tblRole = await _masterRepository.UpdateRole(tblRole);
                RoleDto updatedRole = _mapper.Map<RoleDto>(tblRole);
                _responseDto.Result = updatedRole;
                _logger.LogInformation($"Role updated Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to update role error - {ex.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> DeleteRole(int id)
        {
            try
            {
                _logger.LogInformation($"Delete Role with id {id}");
                await _masterRepository.DeleteRole(id);
                _logger.LogInformation($"Role deleted Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to delete role id {id} error - {ex.Message}";
            }
            return _responseDto;
        }
    }
}

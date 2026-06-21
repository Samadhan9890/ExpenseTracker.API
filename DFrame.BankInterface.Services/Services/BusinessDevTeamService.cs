using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Controllers;
using ExpenseTracker.Services.Migrations;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using ExpenseTracker.Services.Utilities;
using NC.StorageProcessor.Interfaces;
using System.Net;
using System.Security.Claims;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Services
{
	public class BusinessDevTeamService : IBusinessDevTeamService
	{
		private readonly IBusinessDevTeamRepository _businessDevTeamRepo;
        private readonly IClientMasterRepository _clientMasterRepository;
        private readonly IMapper _mapper;
		private ResponseDto _responseDto;
		private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<BusinessDevTeamService> _logger;

        public BusinessDevTeamService(IBusinessDevTeamRepository businessDevTeamRepository,
			IMapper mapper,
			ResponseDto responseDto,
			IHttpContextAccessor contextAccessor,
            ILogger<BusinessDevTeamService> logger,IClientMasterRepository clientMasterRepository)
		{
			_businessDevTeamRepo = businessDevTeamRepository;
			_mapper = mapper;
			_responseDto = responseDto;
			_contextAccessor = contextAccessor;
            _logger = logger;
            _clientMasterRepository = clientMasterRepository;
            
		}

        public async Task<ResponseDto> GetAllBusineesDevTeamAsync()
        {
            try
            {
                List<BusinessDevTeam> businessDevTeams = await _businessDevTeamRepo.GetAllBusineesDevTeamAsync();
                List<BusinessDevTeamDTO> businessDevTeamsDto = _mapper.Map<List<BusinessDevTeamDTO>>(businessDevTeams);
                _responseDto.Result = businessDevTeamsDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while getting all business development teams.");
            }
            return _responseDto;

        }



        public async Task<ResponseDto> GetAllBDAsync()
        {
            try
            {
                List<ClientMaster> businessDevTeams = await _businessDevTeamRepo.GetAllBDTeamAsync();
                List<ClientMasterResponseDto> businessDevTeamsDto = _mapper.Map<List<ClientMasterResponseDto>>(businessDevTeams);
                _responseDto.Result = businessDevTeamsDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while getting all business development teams.");
            }
            return _responseDto;

        }

        public async Task<ResponseDto> GetBusineesDevTeamMemberByIdAsync(int BusineesDevTeamMemberId)
        {
            try
            {
                BusinessDevTeam businessDevTeam = await _businessDevTeamRepo.GetBusineesDevTeamMemberByIdAsync(BusineesDevTeamMemberId);
                // Check if the BusinessDevTeam member was found
                if (businessDevTeam == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"Business Development Team member with ID {BusineesDevTeamMemberId} not found.";
                    return _responseDto;
                }

                BusinessDevTeamDTO businessDevTeamDto = _mapper.Map<BusinessDevTeamDTO>(businessDevTeam);
                // Check if ClientId is valid (not null and greater than 0)
                if (businessDevTeam.ClientId <= 0)
                {
                    // ClientId is invalid, return only the BusinessDevTeam details
                    _responseDto.IsSuccess = true; // Set success response
                    _responseDto.Result = businessDevTeamDto; // Set the result
                    return _responseDto;
                }

                // Retrieve the banking details associated with this client's ID
                var bankingDetails = await _businessDevTeamRepo.GetBankingDetailsByBusinessDevTeamIdAsync(businessDevTeam.BDId);

                // Map and add banking details to the DTO
                businessDevTeamDto.BankingDetails = _mapper.Map<List<ClientBankingDetailsDto>>(bankingDetails);

                _responseDto.IsSuccess = true; // Set success response
                _responseDto.Result = businessDevTeamDto; // Set the result

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while getting business development team member by ID: {BusineesDevTeamMemberId}", BusineesDevTeamMemberId);

            }
            return _responseDto;
        }

        public async Task<ResponseDto> CreateBusinessDevTeamAsync(BusinessDevTeamDTO addBusinessDevTeamDto)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                BusinessDevTeam businessDevTeamToAdd = _mapper.Map<BusinessDevTeam>(addBusinessDevTeamDto);

                _responseDto.Result = await _businessDevTeamRepo.CreateBusinessDevTeamAsync(businessDevTeamToAdd);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to create Business Dev Team Member. - {ex.Message}";
                _logger.LogError(ex, "An error occurred while creating Business Development Team.");

            }
            return _responseDto;
        }
        public async Task<ResponseDto> UpdateBusinessDevTeamAsync(BusinessDevTeamDTO updateBusinessDevTeamDto)
        {
            try
            {
                var userDetails = UserClaimsHelper.GetClaims(_contextAccessor.HttpContext.User.Identity as ClaimsIdentity);
                BusinessDevTeam businessDevTeamToUpdate = _mapper.Map<BusinessDevTeam>(updateBusinessDevTeamDto);
                await _businessDevTeamRepo.UpdateBusinessDevTeamAsync(businessDevTeamToUpdate);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to update Business Dev Team Member. - {ex.Message}";
                _logger.LogError(ex, "An error occurred while updating Business Development Team.");
            }
            return _responseDto;
        }

        public async Task<ResponseDto> CreateBusinessDevTeamBankingDetailsAsync(List<ClientBankingDetailsDto> bankingDetailsDtos)
        {
            try
            {
                var bankingDetailsEntities = new List<ClientBankingDetail>();

                foreach (var bankingDetailDto in bankingDetailsDtos)
                {
                    // Check if BankingType is null or empty and set default to "Referral"
                    if (string.IsNullOrWhiteSpace(bankingDetailDto.BankingType))
                    {
                        bankingDetailDto.BankingType = "Referral";
                    }

                    // Map DTO to Entity
                    var bankingDetailEntity = _mapper.Map<ClientBankingDetail>(bankingDetailDto);
                    bankingDetailsEntities.Add(bankingDetailEntity);
                }

                // Call repository to save the banking details
                await _businessDevTeamRepo.CreateBusinessDevTeamBankingDetailsAsync(bankingDetailsEntities);

                _responseDto.IsSuccess = true;
                _responseDto.Message = "Business Developmet Team Banking details added successfully.";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"An error occurred while adding Business Development Team Banking Details: {ex.Message}";
                _logger.LogError(ex, "An error occurred while creating Business Development Team Banking Details.");
            }

            return _responseDto;
        }

        public async Task<ResponseDto> UpdateBusinessDevTeamBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto)
        {
            try
            {
                var bankingDetail = await _businessDevTeamRepo.UpdateBusinessDevTeamBankingDetailAsync(bankingDetailDto);
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Business Dev Team banking detail updated successfully";
                _responseDto.Result = bankingDetail;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"An error occurred while updating Business Development Team Banking Details: {ex.Message}";
                _logger.LogError(ex, "An error occurred while updating Business Development Team Banking Details.");
            }

            return _responseDto;
        }
        // Service method to call the repository method
        public async Task<bool> CheckIfAadharOrPanExistsAsync(string aadharNumber, string panNumber)
        {
            return await _businessDevTeamRepo.CheckIfAadharOrPanExistsAsync(aadharNumber, panNumber);
        }
    }

}

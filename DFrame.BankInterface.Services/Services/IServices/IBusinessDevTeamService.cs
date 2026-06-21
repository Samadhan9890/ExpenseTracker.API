using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;

namespace ExpenseTracker.Services.Services.IServices
{
	public interface IBusinessDevTeamService
    {
        Task<ResponseDto> GetAllBusineesDevTeamAsync();
        Task<ResponseDto> GetAllBDAsync();
        Task<ResponseDto> GetBusineesDevTeamMemberByIdAsync(int BusineesDevTeamMemberId);       
        Task<ResponseDto> CreateBusinessDevTeamAsync(BusinessDevTeamDTO addBusinessDevTeam);
        Task<ResponseDto> UpdateBusinessDevTeamAsync(BusinessDevTeamDTO updateBusinessDevTeamDto);
        Task<ResponseDto> CreateBusinessDevTeamBankingDetailsAsync(List<ClientBankingDetailsDto> bankingDetailsDtos);
        Task<ResponseDto> UpdateBusinessDevTeamBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto);
        Task<bool> CheckIfAadharOrPanExistsAsync(string aadharNumber, string panNumber);
    }
}

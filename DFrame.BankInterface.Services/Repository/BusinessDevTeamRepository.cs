using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Migrations;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Dashboard;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities;
using static ExpenseTracker.Services.Utilities.ConstantHelper.ConstantHelper;

namespace ExpenseTracker.Services.Repository
{
	public class BusinessDevTeamRepository : IBusinessDevTeamRepository
    {
		private readonly AppDBContext _appDbContext;
		private readonly ILogger<BusinessDevTeamRepository> _logger;
        private readonly IMapper _mapper;
        public BusinessDevTeamRepository(AppDBContext appDBContext,ILogger<BusinessDevTeamRepository> logger,IMapper mapper)
        {
            _appDbContext = appDBContext;
			_logger = logger;
            _mapper = mapper;
        }
        public async Task<List<BusinessDevTeam>> GetAllBusineesDevTeamAsync()
        {
            return await _appDbContext.BusinessDevTeam.AsNoTracking().ToListAsync();
        }

        public async Task<List<ClientMaster>> GetAllBDTeamAsync()
        {
            return await _appDbContext.ClientMasters.AsNoTracking().ToListAsync();
        }
        public async Task<BusinessDevTeam> GetBusineesDevTeamMemberByIdAsync(int BusineesDevTeamMemberId)
        {
            return await _appDbContext.BusinessDevTeam.FindAsync(BusineesDevTeamMemberId);
        }
        public async Task<BusinessDevTeam> CreateBusinessDevTeamAsync(BusinessDevTeam addBusinessDevTeam)
        {
            try
            {
                if(_appDbContext.BusinessDevTeam.Where(b=> b.PanNo == addBusinessDevTeam.PanNo).Any())
                {
                    throw new Exception("Entry with this PAN No already exist in system");
                }
                var res = await _appDbContext.BusinessDevTeam.AddAsync(addBusinessDevTeam);
                await _appDbContext.SaveChangesAsync();
                return res.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while adding new Business Team Member - {ex}");
                throw;
            }
        }
        public async Task UpdateBusinessDevTeamAsync(BusinessDevTeam updateBusinessDevTeam)
        {
            BusinessDevTeam existingBusinessDevTeam = await _appDbContext.BusinessDevTeam.Where(p => p.BDId == updateBusinessDevTeam.BDId)
                                                .FirstOrDefaultAsync();

            _mapper.Map(updateBusinessDevTeam, existingBusinessDevTeam);

            // Explicitly set the Address and Status properties
            existingBusinessDevTeam.Name = updateBusinessDevTeam.Name;
            existingBusinessDevTeam.Address = updateBusinessDevTeam.Address;
            existingBusinessDevTeam.JoiningDate = updateBusinessDevTeam.JoiningDate;
            existingBusinessDevTeam.Status = updateBusinessDevTeam.Status;
            existingBusinessDevTeam.AadharNo=updateBusinessDevTeam.AadharNo;
            existingBusinessDevTeam.PanNo = updateBusinessDevTeam.PanNo;
            _appDbContext.BusinessDevTeam.Update(existingBusinessDevTeam).Property(p => p.BDId).IsModified = false;
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ClientMaster> GetBusinessDevByClientId(int clientId)
        {
           var refferedBy = _appDbContext.ClientMasters.AsNoTracking()
                .Where(c=> c.ClientId == clientId)
                .FirstOrDefaultAsync()
                .GetAwaiter()
                .GetResult()
                .ReferredBy;
            if(refferedBy == null)
            {
                return null;
            }

            var bdDetails = await  _appDbContext.ClientMasters.AsNoTracking()
                .Where(bd => bd.ClientId.ToString() == refferedBy).FirstOrDefaultAsync();


            return bdDetails;

        }

        public async Task CreateBusinessDevTeamBankingDetailsAsync(List<ClientBankingDetail> bankingDetails)
        {
            try
            {
                await _appDbContext.ClientBankingDetails.AddRangeAsync(bankingDetails);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while adding new banking details: {ex}");
                throw;
            }
        }
        public async Task<ClientBankingDetail> UpdateBusinessDevTeamBankingDetailAsync(ClientBankingDetailsDto bankingDetailDto)
        {
            try
            {
                var existingDevTeamBankingDetail = await _appDbContext.ClientBankingDetails.FirstOrDefaultAsync(i => i.Id == bankingDetailDto.Id);

                if (existingDevTeamBankingDetail == null)
                {
                    throw new Exception("DevTeamClientBankingDetail not found.");
                }

                // Update only changed properties
                if (existingDevTeamBankingDetail.BankName != bankingDetailDto.BankName)
                {
                    existingDevTeamBankingDetail.BankName = bankingDetailDto.BankName;
                }

                if (existingDevTeamBankingDetail.AccountHolderName != bankingDetailDto.AccountHolderName)
                {
                    existingDevTeamBankingDetail.AccountHolderName = bankingDetailDto.AccountHolderName;
                }

                if (existingDevTeamBankingDetail.IFSCCode != bankingDetailDto.IFSCCode)
                {
                    existingDevTeamBankingDetail.IFSCCode = bankingDetailDto.IFSCCode;
                }

                if (existingDevTeamBankingDetail.AccountNoOrUpiId != bankingDetailDto.AccountNoOrUpiId)
                {
                    existingDevTeamBankingDetail.AccountNoOrUpiId = bankingDetailDto.AccountNoOrUpiId;
                }

                if (existingDevTeamBankingDetail.Mode != bankingDetailDto.Mode)
                {
                    existingDevTeamBankingDetail.Mode = bankingDetailDto.Mode;
                }

                if (existingDevTeamBankingDetail.Note != bankingDetailDto.Note)
                {
                    existingDevTeamBankingDetail.Note = bankingDetailDto.Note;
                }

                if (existingDevTeamBankingDetail.Status != bankingDetailDto.Status)
                {
                    existingDevTeamBankingDetail.Status = bankingDetailDto.Status;
                }

                // Check if BankingType is empty or null, set default to "Referral"
                var bankingTypeToSet = string.IsNullOrWhiteSpace(bankingDetailDto.BankingType)
                    ? "Referral"
                    : bankingDetailDto.BankingType;

                if (existingDevTeamBankingDetail.BankingType != bankingTypeToSet)
                {
                    existingDevTeamBankingDetail.BankingType = bankingTypeToSet;
                }

                // Save changes only if any field has changed
                await _appDbContext.SaveChangesAsync();

                return existingDevTeamBankingDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while updating ClientBankingDetail - {ex}");
                throw;
            }

        }
        public async Task<List<ClientBankingDetail>> GetBankingDetailsByBusinessDevTeamIdAsync(int bdId)
        {
            return await _appDbContext.ClientBankingDetails
                .Where(b => b.BusinessDevTeamId == bdId && b.Status==true)
                .ToListAsync();
        }

        // Method to check if Aadhar or PAN exists
        public async Task<bool> CheckIfAadharOrPanExistsAsync(string aadharNumber, string panNumber)
        {
            // Query the 'businessdevteam' table to check for Aadhar or PAN existence
            return await _appDbContext.BusinessDevTeam.Where(x=>x.Status == true)
                .AnyAsync(b => b.AadharNo == aadharNumber || b.PanNo == panNumber);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Generic;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using System;
using System.Numerics;

namespace ExpenseTracker.Services.Repository
{
    public class ClientMasterRepository : IClientMasterRepository
	{
		private readonly AppDBContext _appDbContext;
		private readonly IMapper _mapper;

		public ClientMasterRepository(AppDBContext context,IMapper mapper)
		{
			_appDbContext = context;
			_mapper = mapper;
		}

		public async Task<List<ClientMaster>> GetAllClientMastersAsync()
		{
			return await _appDbContext.ClientMasters.AsNoTracking().ToListAsync();
		}

		public async Task<ClientMaster> GetClientMasterByIdAsync(int clientId)
		{
            //return await _appDbContext.ClientMasters.FindAsync(clientId);

            var clientMaster = await _appDbContext.ClientMasters
        .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (clientMaster != null)
            {
                // Fetch the related banking details
                var bankingDetails = await _appDbContext.ClientBankingDetails
                    .Where(b => b.ClientId == clientId && b.Status==true)
                    .ToListAsync();

                // Add the banking details to the client master (assuming you need them together)
                clientMaster.LstClientBankingDetails = bankingDetails;
            }

            return clientMaster;
        }

		public async Task<ClientMaster> AddClientMasterAsync(ClientMaster clientMaster)
		{
			//if (_appDbContext.ClientMasters.Any(c => c.AadharNo == clientMaster.AadharNo))
			//{
			//	throw new Exception("Client master already exists with this Aadhar number");
			//}
			
			_appDbContext.ClientMasters.Add(clientMaster);
			await _appDbContext.SaveChangesAsync();

            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(clientMaster.ClientId,
                null,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.CLIENT_MASTER),
                $"Client Master created - {clientMaster.Name}, ({clientMaster.ClientId})",
                null,
                clientMaster.CreatedBy);

			return clientMaster;
		}

		public async Task DeleteClientMasterAsync(int clientId)
		{
			var clientMaster = await _appDbContext.ClientMasters.FindAsync(clientId);
			if (clientMaster != null)
			{
				clientMaster.Status = false; // Assuming Status is a property to mark the entity as inactive
				await _appDbContext.SaveChangesAsync();
			}
		}

		//public async Task UpdateClientMasterAsync(ClientMaster clientMaster)
		//{
		//	ClientMaster existingClientMaster = await _appDbContext.ClientMasters.Where(p => p.ClientId == clientMaster.ClientId)
		//										.FirstOrDefaultAsync();

		//	_mapper.Map(clientMaster, existingClientMaster);

		//	existingClientMaster.Status = clientMaster.Status;
		//	existingClientMaster.ReferredBy = clientMaster.ReferredBy;
		//	_appDbContext.ClientMasters.Update(existingClientMaster).Property(p => p.ClientId).IsModified = false;
		//	await _appDbContext.SaveChangesAsync();
		//}

        public async Task UpdateClientMasterAsync(ClientMaster clientMaster)
        {
            // Fetch the existing client from the database
            ClientMaster existingClientMaster = await _appDbContext.ClientMasters
                .Where(p => p.ClientId == clientMaster.ClientId)
                .FirstOrDefaultAsync();

            if (existingClientMaster == null)
            {
                // Handle the case where the client does not exist
                throw new KeyNotFoundException("Client not found");
            }

            //// Map properties from clientMaster to existingClientMaster
            //_mapper.Map(clientMaster, existingClientMaster);

            // Update fields only if the new value is different from the existing value

            if (clientMaster.Name != existingClientMaster.Name && !string.IsNullOrEmpty(clientMaster.Name))
            {
                existingClientMaster.Name = clientMaster.Name;
            }

            if (clientMaster.AadharNo != existingClientMaster.AadharNo && !string.IsNullOrEmpty(clientMaster.AadharNo))
            {
                existingClientMaster.AadharNo = clientMaster.AadharNo;
            }

            if (clientMaster.PanNo != existingClientMaster.PanNo && !string.IsNullOrEmpty(clientMaster.PanNo))
            {
                existingClientMaster.PanNo = clientMaster.PanNo;
            }

            if (clientMaster.DOB != existingClientMaster.DOB)
            {
                existingClientMaster.DOB = clientMaster.DOB;
            }

            if (clientMaster.ReferredBy != existingClientMaster.ReferredBy && !string.IsNullOrEmpty(clientMaster.ReferredBy))
            {
                existingClientMaster.ReferredBy = clientMaster.ReferredBy;
            }

            if (clientMaster.PerAddressLine1 != existingClientMaster.PerAddressLine1 && !string.IsNullOrEmpty(clientMaster.PerAddressLine1))
            {
                existingClientMaster.PerAddressLine1 = clientMaster.PerAddressLine1;
            }

            if (clientMaster.PerAddressLine2 != existingClientMaster.PerAddressLine2 && !string.IsNullOrEmpty(clientMaster.PerAddressLine2))
            {
                existingClientMaster.PerAddressLine2 = clientMaster.PerAddressLine2;
            }

            if (clientMaster.PerAddressLine3 != existingClientMaster.PerAddressLine3 && !string.IsNullOrEmpty(clientMaster.PerAddressLine3))
            {
                existingClientMaster.PerAddressLine3 = clientMaster.PerAddressLine3;
            }

            if (clientMaster.PerState != existingClientMaster.PerState && !string.IsNullOrEmpty(clientMaster.PerState))
            {
                existingClientMaster.PerState = clientMaster.PerState;
            }

            if (clientMaster.PerCity != existingClientMaster.PerCity && !string.IsNullOrEmpty(clientMaster.PerCity))
            {
                existingClientMaster.PerCity = clientMaster.PerCity;
            }

            if (clientMaster.PerPinCode != existingClientMaster.PerPinCode)
            {
                existingClientMaster.PerPinCode = clientMaster.PerPinCode;
            }

            if (clientMaster.MailAddressLine1 != existingClientMaster.MailAddressLine1 && !string.IsNullOrEmpty(clientMaster.MailAddressLine1))
            {
                existingClientMaster.MailAddressLine1 = clientMaster.MailAddressLine1;
            }

            if (clientMaster.MailAddressLine2 != existingClientMaster.MailAddressLine2 && !string.IsNullOrEmpty(clientMaster.MailAddressLine2))
            {
                existingClientMaster.MailAddressLine2 = clientMaster.MailAddressLine2;
            }

            if (clientMaster.MailAddressLine3 != existingClientMaster.MailAddressLine3 && !string.IsNullOrEmpty(clientMaster.MailAddressLine3))
            {
                existingClientMaster.MailAddressLine3 = clientMaster.MailAddressLine3;
            }

            if (clientMaster.MailState != existingClientMaster.MailState && !string.IsNullOrEmpty(clientMaster.MailState))
            {
                existingClientMaster.MailState = clientMaster.MailState;
            }

            if (clientMaster.MailCity != existingClientMaster.MailCity && !string.IsNullOrEmpty(clientMaster.MailCity))
            {
                existingClientMaster.MailCity = clientMaster.MailCity;
            }

            if (clientMaster.MailPinCode != existingClientMaster.MailPinCode)
            {
                existingClientMaster.MailPinCode = clientMaster.MailPinCode;
            }

            if (clientMaster.Phone != existingClientMaster.Phone && !string.IsNullOrEmpty(clientMaster.Phone))
            {
                existingClientMaster.Phone = clientMaster.Phone;
            }

            if (clientMaster.Mobile != existingClientMaster.Mobile && !string.IsNullOrEmpty(clientMaster.Mobile))
            {
                existingClientMaster.Mobile = clientMaster.Mobile;
            }

            if (clientMaster.Email != existingClientMaster.Email && !string.IsNullOrEmpty(clientMaster.Email))
            {
                existingClientMaster.Email = clientMaster.Email;
            }

            if (clientMaster.FamilyTag != existingClientMaster.FamilyTag && !string.IsNullOrEmpty(clientMaster.FamilyTag))
            {
                existingClientMaster.FamilyTag = clientMaster.FamilyTag;
            }

            if (clientMaster.Notes != existingClientMaster.Notes && !string.IsNullOrEmpty(clientMaster.Notes))
            {
                existingClientMaster.Notes = clientMaster.Notes;
            }

            if (clientMaster.OfficeAddressLine1 != existingClientMaster.OfficeAddressLine1 && !string.IsNullOrEmpty(clientMaster.OfficeAddressLine1))
            {
                existingClientMaster.OfficeAddressLine1 = clientMaster.OfficeAddressLine1;
            }

            if (clientMaster.OfficeAddressLine2 != existingClientMaster.OfficeAddressLine2 && !string.IsNullOrEmpty(clientMaster.OfficeAddressLine2))
            {
                existingClientMaster.OfficeAddressLine2 = clientMaster.OfficeAddressLine2;
            }

            if (clientMaster.OfficeAddressLine3 != existingClientMaster.OfficeAddressLine3 && !string.IsNullOrEmpty(clientMaster.OfficeAddressLine3))
            {
                existingClientMaster.OfficeAddressLine3 = clientMaster.OfficeAddressLine3;
            }

            if (clientMaster.OfficeState != existingClientMaster.OfficeState && !string.IsNullOrEmpty(clientMaster.OfficeState))
            {
                existingClientMaster.OfficeState = clientMaster.OfficeState;
            }

            if (clientMaster.OfficeCity != existingClientMaster.OfficeCity && !string.IsNullOrEmpty(clientMaster.OfficeCity))
            {
                existingClientMaster.OfficeCity = clientMaster.OfficeCity;
            }

            if (clientMaster.OfficePinCode != existingClientMaster.OfficePinCode)
            {
                existingClientMaster.OfficePinCode = clientMaster.OfficePinCode;
            }

            // Add conditions for new columns
            if (clientMaster.BloodRelation != existingClientMaster.BloodRelation)
            {
                existingClientMaster.BloodRelation = clientMaster.BloodRelation;
            }

            if (clientMaster.IsReferralBonusApplicable != existingClientMaster.IsReferralBonusApplicable)
            {
                existingClientMaster.IsReferralBonusApplicable = clientMaster.IsReferralBonusApplicable;
            }

            if (clientMaster.IsAadharPanLinked != existingClientMaster.IsAadharPanLinked)
            {
                existingClientMaster.IsAadharPanLinked = clientMaster.IsAadharPanLinked;
            }

            if (clientMaster.Status != existingClientMaster.Status)
            {
                existingClientMaster.Status = clientMaster.Status;
            }

            // Check and update AadharAttachmentPath
            if (clientMaster.AadharAttachmentPath != existingClientMaster.AadharAttachmentPath)
            {
                if (clientMaster.AadharAttachmentPath == null)
                {
                    // If the new value is null, do not update the existing value
                    _appDbContext.Entry(existingClientMaster).Property(p => p.AadharAttachmentPath).IsModified = false;
                }
                else
                {
                    // Update the property if the new value is different
                    existingClientMaster.AadharAttachmentPath = clientMaster.AadharAttachmentPath;
                }
            }
            else
            {
                // If the values are the same, mark as not modified
                _appDbContext.Entry(existingClientMaster).Property(p => p.AadharAttachmentPath).IsModified = false;
            }

            // Check and update PanAttachmentPath
            if (clientMaster.PanAttachmentPath != existingClientMaster.PanAttachmentPath)
            {
                if (clientMaster.PanAttachmentPath == null)
                {
                    // If the new value is null, do not update the existing value
                    _appDbContext.Entry(existingClientMaster).Property(p => p.PanAttachmentPath).IsModified = false;
                }
                else
                {
                    // Update the property if the new value is different
                    existingClientMaster.PanAttachmentPath = clientMaster.PanAttachmentPath;
                }
            }
            else
            {
                // If the values are the same, mark as not modified
                _appDbContext.Entry(existingClientMaster).Property(p => p.PanAttachmentPath).IsModified = false;
            }

            // Check and update ProfileImageAttachmentPath
            if (clientMaster.ProfileImageAttachmentPath != existingClientMaster.ProfileImageAttachmentPath)
            {
                if (clientMaster.ProfileImageAttachmentPath == null)
                {
                    // If the new value is null, do not update the existing value
                    _appDbContext.Entry(existingClientMaster).Property(p => p.ProfileImageAttachmentPath).IsModified = false;
                }
                else
                {
                    // Update the property if the new value is different
                    existingClientMaster.ProfileImageAttachmentPath = clientMaster.ProfileImageAttachmentPath;
                }
            }
            else
            {
                // If the values are the same, mark as not modified
                _appDbContext.Entry(existingClientMaster).Property(p => p.ProfileImageAttachmentPath).IsModified = false;
            }

            // Mark specific properties as unchanged
            _appDbContext.Entry(existingClientMaster).Property(p => p.Guid).IsModified = false;          
            _appDbContext.Entry(existingClientMaster).Property(p => p.ClientId).IsModified = false;
            _appDbContext.Entry(existingClientMaster).Property(p => p.CreatedDate).IsModified = false;
            _appDbContext.Entry(existingClientMaster).Property(p => p.CreatedBy).IsModified = false;
           // _appDbContext.Entry(existingClientMaster).Property(p => p.Status).IsModified = false;

            // Save changes to the database
            await _appDbContext.SaveChangesAsync();

            //Add Audit trail
            var genfunc = new GenericFunctions(_appDbContext);
            await genfunc.AddNcAuditTrail(clientMaster.ClientId,
                null,
                null,
                nameof(ConstantHelper.NCAuditModulesEnum.CLIENT_MASTER),
                $"Client Master Updated - {clientMaster.Name}, ({clientMaster.ClientId})",
                null,
                null);
        }

        public async Task<ClientMaster> GetClientMasterByGuidAsync(Guid clientId)
		{
			return await _appDbContext.ClientMasters.Where(c=> c.Guid == clientId).FirstOrDefaultAsync();
		}

        public async Task SaveChangesAsync()
        {
           await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<ClientBankingDetail>> GetBankingDetailsByClientIdAsync(int clientId)
        {
            return await _appDbContext.ClientBankingDetails
                .Where(b => b.ClientId == clientId)
                .ToListAsync();
        }

        public async Task CreateBankingDetails(List<ClientBankingDetail> bankingDetails)
        {
            
            await _appDbContext.ClientBankingDetails.AddRangeAsync(bankingDetails);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
